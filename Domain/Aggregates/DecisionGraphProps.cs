namespace Domain.Aggregates;

public class DecisionGraphProps : BaseEntity
{
    private DecisionGraphProps()
    {

    }
    public DecisionGraphProps(string field, string type)
    {
        Field = field;
        Type = type;
    }

    public string Field { get; private set; } = default!;
    public string Type { get; private set; } = default!;

    public long DecisionGraphId { get; set; }
    public DecisionGraph DecisionGraph { get; set; } = null!;
}
