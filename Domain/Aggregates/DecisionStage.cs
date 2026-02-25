namespace Domain.Aggregates;

public sealed class DecisionStage : BaseEntity
{
    private DecisionStage() { }

    public DecisionStage(long graphId, string name, byte priority
        , HitPolicyType hitPolicy = HitPolicyType.First
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
    public HitPolicyType HitPolicy { get; private set; }
    public StageType StageType { get; private set; }

    public string Name { get; private set; } = default!;
    public byte Priority { get; set; }

    public ICollection<DecisionRule> Rules { get; set; } = [];

    public ICollection<StageConnection> OutgoingConnections { get; set; } = [];
    public ICollection<StageConnection> IncomingConnections { get; set; } = [];

    public void SetStageType(StageType stageType)
    {
        if (stageType == StageType.Switch && HitPolicy != HitPolicyType.First)
        {
            throw new InvalidDataException("In Switch stages, hit policy should be `First` only.");
        }
    }

    public void AddRule(DecisionRule stageRule)
    {
        Rules.Add(stageRule);
    }
}

public enum HitPolicyType
{
    First = 1,
    Collect = 2,
}

public enum StageType
{
    Decision = 1,
    Switch = 2,
}
