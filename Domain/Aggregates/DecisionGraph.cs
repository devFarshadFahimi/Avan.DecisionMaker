namespace Domain.Aggregates;

public sealed class DecisionGraph : BaseEntity
{
    private DecisionGraph() { }

    public DecisionGraph(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; private set; } = default!;

    public ICollection<DecisionStage> Stages { get; set; } = [];
    public ICollection<DecisionGraphProps> DecisionGraphProps { get; set; } = [];
}
