using Domain.Aggregates.Enums;

namespace Domain.Aggregates;

public sealed class DecisionRule : BaseEntity
{
    private DecisionRule() { }

    public DecisionRule(long stageId, LogicalOperator logicalOperator, byte priority)
    {
        StageId = stageId;
        LogicalOperator = logicalOperator;
        Priority = priority;
    }

    public long StageId { get; private set; }
    public DecisionStage Stage { get; private set; } = null!;

    public byte Priority { get; set; }

    public LogicalOperator LogicalOperator { get; private set; }

    public ICollection<DecisionRuleCondition> Conditions { get; private set; } = [];
    public ICollection<DecisionOutput> Outputs { get; private set; } = [];

    public void AddCondition(string op, string value, long propId)
    {
        Conditions.Add(new DecisionRuleCondition(Id, op, value, propId));
    }

    public void AddOutput(string key, string? value)
    {
        Outputs.Add(new DecisionOutput(Id, key, value));
    }
}
