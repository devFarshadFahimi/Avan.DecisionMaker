namespace Application.Common.Models;

public class DecisionGraphPropsDTO
{

    public Guid Id { get; set; }

    public string Field { get; set; } = default!;
    public string Type { get; set; } = default!;

    public Guid DecisionGraphId { get; set; }
    public DecisionGraphDTO DecisionGraph { get; set; } = null!;

    public List<DecisionRuleConditionDTO> DecisionRuleConditions { get; set; } = [];
}