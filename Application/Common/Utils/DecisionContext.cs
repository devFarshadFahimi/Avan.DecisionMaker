using Domain.Aggregates;
using Domain.Aggregates.Enums;

namespace Application.Common.Utils;

public sealed class DecisionContext
{
    private readonly Dictionary<string, object?> _data = [];
    public bool TryGet(string key, out object? value)
    {
        return _data.TryGetValue(key, out value);
    }

    public void Set(string key, object? value)
        => _data[key] = value;

    public T Get<T>(string key)
        => (T)Convert.ChangeType(_data[key]!, typeof(T));

    public bool EvaluateRule(DecisionRule rule)
    {
        if (!rule.Conditions.Any())
            return true;

        var results = rule.Conditions
            .Select(p => p.EvaluateCondition(_data))
            .ToList();

        return rule.LogicalOperator switch
        {
            LogicalOperator.And => results.All(x => x),
            LogicalOperator.Or => results.Any(x => x),
            _ => false
        };
    }
    public void AppendToList(string key, string value)
    {
        if (!_data.TryGetValue(key, out var existing))
        {
            _data[key] = new List<string> { value };
            return;
        }

        if (existing is List<string> list)
        {
            list.Add(value);
            return;
        }

        if (existing is string single)
        {
            _data[key] = new List<string> { single, value };
            return;
        }

        throw new InvalidOperationException(
            $"Property '{key}' is not compatible with List<string>.");
    }
    //private static readonly Dictionary<string, Type> TypeMap = new()
    //{
    //    ["Int32"] = typeof(int),
    //    ["Boolean"] = typeof(bool),
    //    ["String"] = typeof(string),
    //    ["Decimal"] = typeof(decimal)
    //};

    //private bool EvaluateCondition(DecisionRuleCondition condition)
    //{
    //    if (!_data.TryGetValue(condition.Field, out var left))
    //        return false;

    //    var targetType = TypeMap[condition.Type];

    //    var right = Convert.ChangeType(condition.Value, targetType);

    //    left = Convert.ChangeType(left, targetType);

    //    return condition.Operator switch
    //    {
    //        "==" => Equals(left, right),
    //        "!=" => !Equals(left, right),
    //        ">" => ((IComparable)left!).CompareTo(right) > 0,
    //        "<" => ((IComparable)left!).CompareTo(right) < 0,
    //        ">=" => ((IComparable)left!).CompareTo(right) >= 0,
    //        "<=" => ((IComparable)left!).CompareTo(right) <= 0,
    //        _ => throw new NotSupportedException(condition.Operator)
    //    };
    //}

    public IReadOnlyDictionary<string, object?> Snapshot()
        => new Dictionary<string, object?>(_data);
}