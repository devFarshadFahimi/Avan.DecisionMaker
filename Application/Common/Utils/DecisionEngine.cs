using Domain.Aggregates;

namespace Application.Common.Utils;

//public sealed class DecisionEngine : IDecisionEngine
//{
//    public async Task ExecuteAsync(
//    DecisionGraph graph,
//    DecisionContext context,
//    CancellationToken cancellationToken = default)
//    {
//        ArgumentNullException.ThrowIfNull(graph);
//        ArgumentNullException.ThrowIfNull(context);

//        // برای lookup سریع
//        var stages = graph.Stages.ToDictionary(x => x.Id);

//        // محاسبه indegree (تعداد Incoming)
//        var remainingDeps = stages.Values
//            .ToDictionary(
//                s => s.Id,
//                s => s.IncomingConnections.Count);

//        // صف stage های آماده اجرا
//        var readyQueue = new Queue<DecisionStage>(
//            stages.Values.Where(s => remainingDeps[s.Id] == 0));

//        var executedCount = 0;

//        while (readyQueue.Count > 0)
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var stage = readyQueue.Dequeue();

//            //// اجرای Stage -- old style
//            //ExecuteStage(stage, context);

//            HitPoilicyBasedExecuteStage(stage, context);

//            executedCount++;

//            // کاهش dependency های مقصد
//            foreach (var connection in stage.OutgoingConnections)
//            {
//                var targetId = connection.ToStageId;

//                remainingDeps[targetId]--;

//                if (remainingDeps[targetId] == 0)
//                {
//                    readyQueue.Enqueue(stages[targetId]);
//                }
//            }
//        }

//        // اگر همه اجرا نشدن یعنی cycle وجود دارد
//        if (executedCount != stages.Count)
//            throw new InvalidOperationException("Graph contains cycle.");

//        await Task.CompletedTask;
//    }

//    private static void ExecuteStage(
//        DecisionStage stage,
//        DecisionContext context)
//    {
//        foreach (var rule in stage.Rules.OrderBy(p => p.Priority))
//        {
//            if (!context.EvaluateRule(rule))
//                continue;

//            foreach (var output in rule.Outputs)
//                context.Set(output.Key, ParseValue(output.Value));

//            return;
//        }

//        throw new InvalidOperationException(
//            $"No rule matched in stage '{stage.Name}'.");
//    }

//    private static void HitPoilicyBasedExecuteStage(
//    DecisionStage stage,
//    DecisionContext context)
//    {
//        // مرتب‌سازی بر اساس اولویت Rule
//        var orderedRules = stage.Rules
//            .OrderBy(r => r.Priority)
//            .ToList();

//        // همه Ruleهای Match شده
//        var matchedRules = new List<DecisionRule>();

//        foreach (var rule in orderedRules)
//        {
//            if (context.EvaluateRule(rule))
//                matchedRules.Add(rule);
//        }

//        if (matchedRules.Count == 0)
//            throw new InvalidOperationException(
//                $"No rule matched in stage '{stage.Name}'.");

//        switch (stage.HitPolicy)
//        {
//            case HitPolicy.First:
//                ApplyOutputs(matchedRules.First(), context);
//                return;

//            case HitPolicy.Unique:
//                if (matchedRules.Count != 1)
//                    throw new InvalidOperationException(
//                        $"Stage '{stage.Name}' requires exactly one matching rule.");
//                ApplyOutputs(matchedRules[0], context);
//                return;

//            case HitPolicy.Any:
//                EnsureSameOutputs(matchedRules);
//                ApplyOutputs(matchedRules[0], context);
//                return;

//            //case HitPolicy.Collect:
//            //    foreach (var rule in matchedRules)
//            //        ApplyOutputs(rule, context);
//            //    return;
//            case HitPolicy.Collect:
//                foreach (var rule in matchedRules)
//                    ApplyOutputs(rule, context, appendMode: true);
//                return;

//            case HitPolicy.CollectCount:
//                context.Set($"{stage.Name}_count", matchedRules.Count);
//                return;

//            case HitPolicy.CollectSum:
//                Aggregate(matchedRules, context, AggregateType.Sum);
//                return;

//            case HitPolicy.CollectMin:
//                Aggregate(matchedRules, context, AggregateType.Min);
//                return;

//            case HitPolicy.CollectMax:
//                Aggregate(matchedRules, context, AggregateType.Max);
//                return;

//            case HitPolicy.Priority:
//                var highest = matchedRules
//                    .OrderByDescending(r => r.Priority)
//                    .First();
//                ApplyOutputs(highest, context);
//                return;

//            default:
//                throw new NotSupportedException(
//                    $"HitPolicy '{stage.HitPolicy}' is not supported.");
//        }
//    }


//    #region Old Version
//    private static object? ParseValue(string? value)
//    {
//        if (value is null)
//            return null;

//        if (bool.TryParse(value, out var b)) return b;
//        if (int.TryParse(value, out var i)) return i;
//        if (decimal.TryParse(value, out var d)) return d;

//        return value;
//    }
//    #endregion


//    #region New methods

//    private static void ApplyOutputs(
//    DecisionRule rule,
//    DecisionContext context,
//    bool appendMode = false)
//    {
//        foreach (var output in rule.Outputs)
//        {
//            var parsed = ParseValue(output.Value);

//            if (appendMode && parsed is string str)
//            {
//                context.AppendToList(output.Key, str);
//            }
//            else
//            {
//                context.Set(output.Key, parsed);
//            }
//        }
//    }

//    private static void EnsureSameOutputs(
//        IReadOnlyList<DecisionRule> rules)
//    {
//        var first = rules[0].Outputs
//            .Select(o => (o.Key, o.Value))
//            .OrderBy(x => x.Key)
//            .ToList();

//        foreach (var rule in rules.Skip(1))
//        {
//            var current = rule.Outputs
//                .Select(o => (o.Key, o.Value))
//                .OrderBy(x => x.Key)
//                .ToList();

//            if (!first.SequenceEqual(current))
//                throw new InvalidOperationException(
//                    "ANY hit policy violated. Outputs differ.");
//        }
//    }

//    private static void Aggregate(
//        IReadOnlyList<DecisionRule> rules,
//        DecisionContext context,
//        AggregateType type)
//    {
//        var grouped = rules
//            .SelectMany(r => r.Outputs)
//            .GroupBy(o => o.Key);

//        foreach (var group in grouped)
//        {
//            var values = group
//                .Select(o => Convert.ToDecimal(ParseValue(o.Value)))
//                .ToList();

//            decimal result = type switch
//            {
//                AggregateType.Sum => values.Sum(),
//                AggregateType.Min => values.Min(),
//                AggregateType.Max => values.Max(),
//                _ => throw new NotSupportedException()
//            };

//            context.Set(group.Key, result);
//        }
//    }

//    private enum AggregateType
//    {
//        Sum,
//        Min,
//        Max
//    }

//    #endregion
//}


public sealed class DecisionEngine : IDecisionEngine
{
    public async Task ExecuteAsync(
        DecisionGraph graph,
        DecisionContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(context);

        var stages = graph.Stages.ToDictionary(x => x.Id);

        var remainingDeps = stages.Values
            .ToDictionary(
                s => s.Id,
                s => s.IncomingConnections.Count);

        var readyQueue = new Queue<DecisionStage>(
            stages.Values.Where(s => remainingDeps[s.Id] == 0));

        var executedCount = 0;

        while (readyQueue.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var stage = readyQueue.Dequeue();

            ExecuteStageByHitPolicy(stage, context);

            executedCount++;

            foreach (var connection in stage.OutgoingConnections)
            {
                var targetId = connection.ToStageId;

                remainingDeps[targetId]--;

                if (remainingDeps[targetId] == 0)
                    readyQueue.Enqueue(stages[targetId]);
            }
        }

        if (executedCount != stages.Count)
            throw new InvalidOperationException("Graph contains cycle.");

        await Task.CompletedTask;
    }

    private static void ExecuteStageByHitPolicy(
        DecisionStage stage,
        DecisionContext context)
    {
        var orderedRules = stage.Rules
            .OrderBy(r => r.Priority)
            .ToList();

        var matchedRules = orderedRules
            .Where(context.EvaluateRule)
            .ToList();

        if (matchedRules.Count == 0)
            throw new InvalidOperationException(
                $"No rule matched in stage '{stage.Name}'.");

        switch (stage.HitPolicy)
        {
            case HitPolicy.First:
                ApplyFirst(matchedRules[0], context);
                break;

            case HitPolicy.Collect:
                ApplyCollect(matchedRules, context);
                break;

            default:
                throw new NotSupportedException(
                    $"HitPolicy '{stage.HitPolicy}' is not supported.");
        }
    }

    #region HitPolicy Implementations

    private static void ApplyFirst(
        DecisionRule rule,
        DecisionContext context)
    {
        foreach (var output in rule.Outputs)
        {
            var parsed = ParseValue(output.Value);
            context.Set(output.Key, parsed);
        }
    }

    private static void ApplyCollect(
        IReadOnlyList<DecisionRule> rules,
        DecisionContext context)
    {
        foreach (var rule in rules)
        {
            foreach (var output in rule.Outputs)
            {
                var parsed = ParseValue(output.Value);

                // اگر قبلاً لیست وجود ندارد، ایجاد شود
                if (!context.TryGet(output.Key, out var existing))
                {
                    context.Set(output.Key, new List<object?> { parsed });
                    continue;
                }

                // اگر قبلاً لیست بوده
                if (existing is List<object?> list)
                {
                    list.Add(parsed);
                }
                else
                {
                    // اگر قبلاً مقدار single بوده، تبدیل به لیست شود
                    var newList = new List<object?> { existing, parsed };
                    context.Set(output.Key, newList);
                }
            }
        }
    }

    #endregion

    private static object? ParseValue(string? value)
    {
        if (value is null)
            return null;

        if (bool.TryParse(value, out var b)) return b;
        if (int.TryParse(value, out var i)) return i;
        if (decimal.TryParse(value, out var d)) return d;

        return value;
    }
}