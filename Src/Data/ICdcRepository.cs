namespace SecureTrack;

public interface ICdcRepository
{
    Task LogChangeAsync(CdcEntry entry);
    Task LogChangesAsync(List<CdcEntry> entries);
}