using SecureTrack.Services.Interfaces;

namespace SecureTrack;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class SecureTrackDbContext : DbContext
{
    private readonly IDataIntegrityService _integrityService;
    private readonly ICdcRepository _cdcRepository;

    public SecureTrackDbContext(
        DbContextOptions<SecureTrackDbContext> options,
        IDataIntegrityService integrityService,
        ICdcRepository cdcRepository)
        : base(options)
    {
        _integrityService = integrityService;
        _cdcRepository = cdcRepository;
    }

    public DbSet<CdcEntry> CdcEntries { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            await ProcessEntryAsync(entry);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessEntryAsync(EntityEntry entry)
    {
        var entity = entry.Entity;
        var entityName = entity.GetType().Name;
        var entityId = ExtractEntityId(entity);
        var user = "SYSTEM"; // Replace with real user context if available

        string oldValue = null;
        string newValue = null;

        switch (entry.State)
        {
            case EntityState.Added:
                newValue = _integrityService.Serialize(entity);
                await _integrityService.CheckIntegrityAsync(entity);
                await _cdcRepository.LogChangeAsync(new CdcEntry
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    OperationType = "Insert",
                    NewValue = newValue,
                });
                break;

            case EntityState.Modified:
                oldValue = _integrityService.Serialize(entry.OriginalValues.ToObject());
                newValue = _integrityService.Serialize(entity);
                await _integrityService.CheckIntegrityAsync(entity);
                await _cdcRepository.LogChangeAsync(new CdcEntry
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    OperationType = "Update",
                    OldValue = oldValue,
                    NewValue = newValue,
                });
                break;

            case EntityState.Deleted:
                oldValue = _integrityService.Serialize(entity);
                await _cdcRepository.LogChangeAsync(new CdcEntry
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    OperationType = "Delete",
                    OldValue = oldValue,
                });
                break;
        }
    }

    private string ExtractEntityId(object entity)
    {
        // Simple reflection lookup for 'Id' property
        var idProp = entity.GetType().GetProperty("Id");
        return idProp?.GetValue(entity)?.ToString() ?? "Unknown";
    }
}

