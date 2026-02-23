using Domain.Aggregates;

namespace Application.Common.Utils;

public sealed class DecisionEngine : IDecisionEngine
{
    public async Task ExecuteAsync(
    DecisionGraph graph,
    DecisionContext context,
    CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(context);

        // برای lookup سریع
        var stages = graph.Stages.ToDictionary(x => x.Id);

        // محاسبه indegree (تعداد Incoming)
        var remainingDeps = stages.Values
            .ToDictionary(
                s => s.Id,
                s => s.IncomingConnections.Count);

        // صف stage های آماده اجرا
        var readyQueue = new Queue<DecisionStage>(
            stages.Values.Where(s => remainingDeps[s.Id] == 0));

        var executedCount = 0;

        while (readyQueue.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var stage = readyQueue.Dequeue();

            // اجرای Stage
            ExecuteStage(stage, context);

            executedCount++;

            // کاهش dependency های مقصد
            foreach (var connection in stage.OutgoingConnections)
            {
                var targetId = connection.ToStageId;

                remainingDeps[targetId]--;

                if (remainingDeps[targetId] == 0)
                {
                    readyQueue.Enqueue(stages[targetId]);
                }
            }
        }

        // اگر همه اجرا نشدن یعنی cycle وجود دارد
        if (executedCount != stages.Count)
            throw new InvalidOperationException("Graph contains cycle.");

        await Task.CompletedTask;
    }

    //public async Task ExecuteAsync(
    //    DecisionGraph graph,
    //    DecisionContext context,
    //    CancellationToken cancellationToken = default)
    //{
    //    var executed = new HashSet<Guid>();

    //    var remainingDeps = graph.Stages
    //        .ToDictionary(
    //            s => s.Id,
    //            s => s.Predecessors.Count);

    //    var readyQueue = new Queue<DecisionStage>();

    //    var startStages = graph.Stages
    //        .Where(s => !s.Predecessors.Any())
    //        .ToList();

    //    foreach (var stage in startStages)
    //        readyQueue.Enqueue(stage);

    //    while (readyQueue.Count > 0)
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();

    //        var stage = readyQueue.Dequeue();

    //        if (remainingDeps[stage.Id] > 0)
    //        {
    //            readyQueue.Enqueue(stage);
    //            continue;
    //        }

    //        if (executed.Contains(stage.Id))
    //            continue;

    //        ExecuteStage(stage, context);

    //        executed.Add(stage.Id);

    //        foreach (var connection in stage.Connections)
    //        {
    //            remainingDeps[connection.ToStageId]--;

    //            var nextStage = graph.Stages
    //                .Single(x => x.Id == connection.ToStageId);

    //            readyQueue.Enqueue(nextStage);
    //        }
    //    }

    //    await Task.CompletedTask;
    //}

    private static void ExecuteStage(
        DecisionStage stage,
        DecisionContext context)
    {
        foreach (var rule in stage.Rules.OrderBy(p => p.Priority))
        {
            if (!context.EvaluateRule(rule))
                continue;

            foreach (var output in rule.Outputs)
                context.Set(output.Key, ParseValue(output.Value));

            return;
        }

        throw new InvalidOperationException(
            $"No rule matched in stage '{stage.Name}'.");
    }

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

