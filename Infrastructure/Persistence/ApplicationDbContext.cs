using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<DecisionGraph> DecisionGraphs => Set<DecisionGraph>();
    public DbSet<DecisionStage> DecisionStages => Set<DecisionStage>();
    public DbSet<DecisionRule> DecisionRules => Set<DecisionRule>();
    public DbSet<DecisionRuleCondition> DecisionRuleConditions => Set<DecisionRuleCondition>();
    public DbSet<DecisionOutput> DecisionOutputs => Set<DecisionOutput>();
    public DbSet<StageConnection> StageConnection => Set<StageConnection>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
