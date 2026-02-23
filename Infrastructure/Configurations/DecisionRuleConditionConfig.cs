using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DecisionRuleConditionConfig : IEntityTypeConfiguration<DecisionRuleCondition>
{
    public void Configure(EntityTypeBuilder<DecisionRuleCondition> builder)
    {
        builder.HasOne(p => p.Rule)
            .WithMany(p => p.Conditions)
            .HasForeignKey(p => p.RuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
