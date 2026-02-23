using Domain.Aggregates;

namespace Application.Common.Utils;

public interface IDecisionEngine
{
    Task ExecuteAsync(
        DecisionGraph graph,
        DecisionContext context,
        CancellationToken cancellationToken = default);
}

