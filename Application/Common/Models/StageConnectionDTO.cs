namespace Application.Common.Models;

public sealed class StageConnectionDTO
{
    public Guid Id { get; set; }

    public Guid GraphId { get; set; }

    public Guid FromStageId { get; set; }
    public DecisionStageDTO FromStage { get; set; } = null!;

    public Guid ToStageId { get; set; }
    public DecisionStageDTO ToStage { get; set; } = null!;
}