using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DecisionStageConfig : IEntityTypeConfiguration<DecisionStage>
{
    public void Configure(EntityTypeBuilder<DecisionStage> builder)
    {
        builder.HasOne(p => p.Graph)
            .WithMany(p => p.Stages)
            .HasForeignKey(p => p.GraphId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
