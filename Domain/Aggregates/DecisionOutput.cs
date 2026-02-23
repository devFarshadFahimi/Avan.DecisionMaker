namespace Domain.Aggregates;

public sealed class DecisionOutput : BaseEntity
{
    private DecisionOutput() { } // EF

    internal DecisionOutput(long ruleId, string key, string? value)
    {
        RuleId = ruleId;
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value;
    }

    public long RuleId { get; private set; }
    public DecisionRule Rule { get; private set; } = null!;

    public string Key { get; private set; } = default!;
    public string? Value { get; private set; }
}
