namespace Domain.Aggregates;

public sealed class DecisionStage : BaseEntity
{
    private DecisionStage() { }

    public DecisionStage(long graphId, string name, byte priority)
    {
        GraphId = graphId;
        Priority = priority;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public long GraphId { get; private set; }
    public DecisionGraph Graph { get; private set; } = null!;

    public string Name { get; private set; } = default!;
    public byte Priority { get; set; }

    public ICollection<DecisionRule> Rules { get; set; } = [];

    public ICollection<StageConnection> OutgoingConnections { get; set; } = [];
    public ICollection<StageConnection> IncomingConnections { get; set; } = [];

    public void AddRule(DecisionRule stageRule)
    {
        Rules.Add(stageRule);
    }
}
