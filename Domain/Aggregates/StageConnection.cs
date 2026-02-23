namespace Domain.Aggregates;

public sealed class StageConnection : BaseEntity
{
    private StageConnection() { }

    public StageConnection(long graphId, long fromStageId, long toStageId)
    {
        GraphId = graphId;
        FromStageId = fromStageId;
        ToStageId = toStageId;
    }

    public long GraphId { get; private set; }

    public long FromStageId { get; private set; }
    public DecisionStage FromStage { get; private set; } = null!;

    public long ToStageId { get; private set; }
    public DecisionStage ToStage { get; private set; } = null!;
}