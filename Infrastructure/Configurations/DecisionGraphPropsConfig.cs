using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class DecisionGraphPropsConfig : IEntityTypeConfiguration<DecisionGraphProps>
{
    public void Configure(EntityTypeBuilder<DecisionGraphProps> builder)
    {
        builder.HasOne(p => p.DecisionGraph)
            .WithMany(p => p.DecisionGraphProps)
            .HasForeignKey(p => p.DecisionGraphId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

