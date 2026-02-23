using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class StageConnectionConfig : IEntityTypeConfiguration<StageConnection>
{
    public void Configure(EntityTypeBuilder<StageConnection> builder)
    {
        builder
           .HasOne(x => x.FromStage)
           .WithMany(x => x.OutgoingConnections)
           .HasForeignKey(x => x.FromStageId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ToStage)
            .WithMany(x => x.IncomingConnections)
            .HasForeignKey(x => x.ToStageId)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.HasOne(p => p.FromStage)
        //    .WithMany(p => p.Connections)
        //    .HasForeignKey(p => p.FromStageId)
        //    .OnDelete(DeleteBehavior.NoAction);

        ////builder.HasOne(p => p.ToStage)
        ////    .WithMany(p => p.Connections)
        ////    .HasForeignKey(p => p.ToStageId)
        ////    .OnDelete(DeleteBehavior.NoAction);
    }
}
