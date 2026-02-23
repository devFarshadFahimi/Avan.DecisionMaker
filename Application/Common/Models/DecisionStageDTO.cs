namespace Application.Common.Models;

public sealed class DecisionStageDTO
{
    public Guid Id { get; set; }

    public Guid GraphId { get; set; }
    //public DecisionGraphDTO Graph { get; set; } = null!;

    public string Name { get; set; } = default!;
    public byte Order { get; set; }
    //public List<DecisionRuleDTO> Rules { get; set; } = [];
    //public List<StageConnectionDTO> OutgoingConnections { get; set; } = [];
    //public List<StageConnectionDTO> IncomingConnections { get; set; } = [];

}
