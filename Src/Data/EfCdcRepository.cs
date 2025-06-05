namespace SecureTrack;

public class EfCdcRepository : ICdcRepository
{
    private readonly SecureTrackDbContext _dbContext;

    public EfCdcRepository(SecureTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogChangeAsync(CdcEntry entry)
    {
        await _dbContext.CdcEntries.AddAsync(entry);
        await _dbContext.SaveChangesAsync();
    } 
    public async Task LogChangesAsync(List<CdcEntry> entries)
    {
        await _dbContext.CdcEntries.AddRangeAsync(entries);
        await _dbContext.SaveChangesAsync();
    }
}