namespace Application.Common.Utils;

//public static class DecisionRowPropertyExtension
//{
//    public static Func<DecisionContext, bool> Build(this IReadOnlyCollection<DecisionRowProperty> properties)
//    {
//        if (properties.Count == 0)
//        {
//            return _ => true;
//        }

//        var contextParam = Expression.Parameter(typeof(DecisionContext), "c");

//        Expression? finalBody = null;

//        foreach (var prop in properties)
//        {
//            var propertyExpression = BuildSingleExpression(contextParam, prop);

//            finalBody = finalBody == null
//                ? propertyExpression
//                : Expression.AndAlso(finalBody, propertyExpression);
//        }

//        var lambda = Expression.Lambda<Func<DecisionContext, bool>>(
//            finalBody!,
//            contextParam);

//        return lambda.Compile();
//    }

//    private static Expression BuildSingleExpression(
//        ParameterExpression contextParam,
//        DecisionRowProperty prop)
//    {
//        var getMethod = typeof(DecisionContext)
//            .GetMethod(nameof(DecisionContext.Get))!
//            .MakeGenericMethod(GetClrType(prop.Type));

//        var fieldExpr = Expression.Call(
//            contextParam,
//            getMethod,
//            Expression.Constant(prop.Field));

//        var constantValue = Expression.Constant(
//            ConvertToType(prop.Value, prop.Type),
//            GetClrType(prop.Type));

//        return prop.Operator switch
//        {
//            "==" => Expression.Equal(fieldExpr, constantValue),
//            "!=" => Expression.NotEqual(fieldExpr, constantValue),
//            ">" => Expression.GreaterThan(fieldExpr, constantValue),
//            "<" => Expression.LessThan(fieldExpr, constantValue),
//            ">=" => Expression.GreaterThanOrEqual(fieldExpr, constantValue),
//            "<=" => Expression.LessThanOrEqual(fieldExpr, constantValue),

//            _ => throw new NotSupportedException(
//                $"Operator '{prop.Operator}' not supported")
//        };
//    }

//    private static Type GetClrType(string type) =>
//        type switch
//        {
//            "int" => typeof(int),
//            "bool" => typeof(bool),
//            "string" => typeof(string),
//            "decimal" => typeof(decimal),
//            _ => throw new NotSupportedException($"Type '{type}' not supported")
//        };

//    private static object ConvertToType(string value, string type) =>
//        type switch
//        {
//            "int" => int.Parse(value),
//            "bool" => bool.Parse(value),
//            "decimal" => decimal.Parse(value),
//            "string" => value,
//            _ => throw new NotSupportedException($"Type '{type}' not supported")
//        };
//}

