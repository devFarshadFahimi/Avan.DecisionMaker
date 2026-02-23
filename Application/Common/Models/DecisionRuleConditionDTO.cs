namespace Application.Common.Models;

public sealed class DecisionRuleConditionDTO
{
    public Guid Id { get; set; }
    public Guid RuleId { get; set; }
    public DecisionRuleDTO Rule { get; set; } = null!;

    public string Field { get; set; } = string.Empty;
    public Guid? PropertyId { get; set; }
    public DecisionGraphPropsDTO? Property { get; set; }
    public string Operator { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
