namespace SecureTrack;

public class CdcEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string EntityName { get; set; }

    public string EntityId { get; set; }

    public string OperationType { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }


    public DateTime ChangedAt { get; set; }
}