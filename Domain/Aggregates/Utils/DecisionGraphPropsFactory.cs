using System.Reflection;

namespace Domain.Aggregates.Utils;

public static class DecisionGraphPropsFactory
{
    public static ICollection<DecisionGraphProps> CreateFromModel<T>()
    {
        var type = typeof(T);

        var props = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => IsSupportedType(p.PropertyType))
            .Select(p =>
                new DecisionGraphProps(
                    field: p.Name,
                    type: NormalizeType(p.PropertyType)))
            .ToList();

        return props;
    }

    private static bool IsSupportedType(Type type)
    {
        var underlying = Nullable.GetUnderlyingType(type) ?? type;

        return underlying == typeof(int)
            || underlying == typeof(bool)
            || underlying == typeof(decimal)
            || underlying == typeof(string)
            || underlying == typeof(double)
            || underlying == typeof(float);
    }

    private static string NormalizeType(Type type)
    {
        var underlying = Nullable.GetUnderlyingType(type) ?? type;

        return underlying.Name switch
        {
            "Int32" => "Int32",
            "Boolean" => "Boolean",
            "Decimal" => "Decimal",
            "String" => "String",
            "Double" => "Double",
            "Single" => "Single",
            _ => throw new NotSupportedException(
                $"Type {underlying.Name} is not supported.")
        };
    }
}