namespace Application.Common.Models;

public sealed class DecisionOutputDTO
{
    public Guid Id { get; set; }

    public Guid RuleId { get; set; }
    public DecisionRuleDTO Rule { get; set; } = null!;

    public string Key { get; set; } = default!;
    public string? Value { get; set; }
}
