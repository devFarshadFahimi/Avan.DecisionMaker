namespace Domain.Aggregates;

public sealed class DecisionStage : BaseEntity
{
    private DecisionStage() { }

    public DecisionStage(long graphId, string name, byte priority
        , HitPolicy hitPolicy = HitPolicy.First
        , StageType stageType = StageType.Decision)
    {
        GraphId = graphId;
        Priority = priority;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HitPolicy = hitPolicy;
        StageType = stageType;
    }

    public long GraphId { get; private set; }
    public DecisionGraph Graph { get; private set; } = null!;
    public HitPolicy HitPolicy { get; private set; } = HitPolicy.First;
    public StageType StageType { get; private set; }

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

public enum HitPolicy
{
    First = 1,
    Collect = 2,
}

public enum StageType
{
    Decision = 1,
    Switch = 2,
}
