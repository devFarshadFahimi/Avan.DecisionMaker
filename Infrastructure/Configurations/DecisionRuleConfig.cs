using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DecisionRuleConfig : IEntityTypeConfiguration<DecisionRule>
{
    public void Configure(EntityTypeBuilder<DecisionRule> builder)
    {
        builder.HasOne(p => p.Stage)
            .WithMany(p => p.Rules)
            .HasForeignKey(p => p.StageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
