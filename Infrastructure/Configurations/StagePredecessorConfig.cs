namespace Infrastructure.Configurations;

//public class StagePredecessorConfig : IEntityTypeConfiguration<StagePredecessor>
//{
//    public void Configure(EntityTypeBuilder<StagePredecessor> builder)
//    {
//        builder.HasOne(p => p.FromStage)
//            .WithMany(p => p.Predecessors)
//            .HasForeignKey(p => p.FromStageId)
//            .OnDelete(DeleteBehavior.Restrict);

//        //builder.HasOne(p => p.Stage)
//        //    .WithMany(p => p.Predecessors)
//        //    .HasForeignKey(p => p.StageId)
//        //    .OnDelete(DeleteBehavior.Restrict);
//    }
//}
