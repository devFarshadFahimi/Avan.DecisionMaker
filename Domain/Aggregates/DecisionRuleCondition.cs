namespace Domain.Aggregates;

public sealed class DecisionRuleCondition : BaseEntity
{
    private DecisionRuleCondition() { }

    public DecisionRuleCondition(
        long ruleId,
        string @operator,
        string value,
        long propertyId
        )
    {
        RuleId = ruleId;
        Operator = @operator;
        Value = value;
        PropertyId = propertyId;
    }


    public long RuleId { get; private set; }
    public DecisionRule Rule { get; private set; } = null!;

    public long? PropertyId { get; private set; }
    public DecisionGraphProps? Property { get; private set; }
    public string Operator { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;

    public bool EvaluateCondition(Dictionary<string, object?> contextData)
    {
        if (Property is null)
            return false;

        if (!contextData.TryGetValue(Property.Field, out var left))
            return false;

        if (left is null)
            return false;

        // map type
        if (!TypeMap.TryGetValue(Property.Type, out var targetType))
            targetType = typeof(string); // fallback

        object right;
        try
        {
            if (targetType == typeof(string))
            {
                right = Value; // مستقیم string بگیر
            }
            else
            {
                right = Convert.ChangeType(Value, targetType);
                left = Convert.ChangeType(left, targetType)!;
            }
        }
        catch
        {
            return false; // conversion fail → false
        }

        return Operator switch
        {
            "==" => Equals(left, right),
            "!=" => !Equals(left, right),
            ">" => ((IComparable)left!).CompareTo(right) > 0,
            ">=" => ((IComparable)left!).CompareTo(right) >= 0,
            "<" => ((IComparable)left!).CompareTo(right) < 0,
            "<=" => ((IComparable)left!).CompareTo(right) <= 0,
            _ => throw new NotSupportedException(Operator)
        };
    }

    private static readonly Dictionary<string, Type> TypeMap = new()
    {
        ["int"] = typeof(int),
        ["bool"] = typeof(bool),
        ["string"] = typeof(string),
        ["long"] = typeof(long),
        ["byte"] = typeof(byte),
        ["short"] = typeof(short),
        ["decimal"] = typeof(decimal)
    };

    //public bool EvaluateCondition(Dictionary<string, object?> contextData)
    //{
    //    if (Property is null)
    //    {
    //        return false;
    //    }

    //    if (!contextData.TryGetValue(Property.Field, out var left))
    //        return false;

    //    var targetType = TypeMap[Property.Type];

    //    var right = Convert.ChangeType(Value, targetType);

    //    left = Convert.ChangeType(left, targetType);

    //    return Operator switch
    //    {
    //        "==" => Equals(left, right),
    //        "!=" => !Equals(left, right),
    //        ">" => ((IComparable)left!).CompareTo(right) > 0,
    //        "<" => ((IComparable)left!).CompareTo(right) < 0,
    //        ">=" => ((IComparable)left!).CompareTo(right) >= 0,
    //        "<=" => ((IComparable)left!).CompareTo(right) <= 0,
    //        _ => throw new NotSupportedException(Operator)
    //    };
    //}
}

