namespace Application.Common.Models;

public sealed class DecisionGraphDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public List<DecisionStageDTO> Stages { get; set; } = [];
    public List<DecisionGraphPropsDTO> DecisionGraphProps { get; set; } = [];
}
