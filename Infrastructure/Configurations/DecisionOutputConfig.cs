using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DecisionOutputConfig : IEntityTypeConfiguration<DecisionOutput>
{
    public void Configure(EntityTypeBuilder<DecisionOutput> builder)
    {
        builder.HasOne(p => p.Rule)
            .WithMany(p => p.Outputs)
            .HasForeignKey(p => p.RuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

