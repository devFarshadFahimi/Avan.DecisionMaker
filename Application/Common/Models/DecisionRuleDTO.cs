using Domain.Aggregates.Enums;

namespace Application.Common.Models;

public sealed class DecisionRuleDTO
{
    public Guid Id { get; set; }
    public Guid StageId { get; set; }
    public DecisionStageDTO Stage { get; set; } = null!;

    public LogicalOperator LogicalOperator { get; set; }

    public List<DecisionRuleConditionDTO> Conditions { get; set; } = [];
    public List<DecisionOutputDTO> Outputs { get; set; } = [];
}
