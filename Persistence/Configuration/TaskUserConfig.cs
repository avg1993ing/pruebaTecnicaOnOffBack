using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class TaskUserConfig : IEntityTypeConfiguration<TaskUser>
    {
        public void Configure(EntityTypeBuilder<TaskUser> builder)
        {
            builder.HasKey(e => e.id);
            builder.ToTable("taskuser");
            builder.Property(e => e.id).HasColumnName("idtaskuser");
            builder.Property(e => e.NameTask).HasColumnName("nametask");
            builder.Property(e => e.DateTask).HasColumnName("datetask");
            builder.Property(e => e.Complete).HasColumnName("complete");


            builder.HasOne(e => e.IdUsersNavigation)
          .WithMany(e => e.TaskUsers)
          .HasForeignKey(e => e.idUsers)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("fk_taus_user");

        }
    }
}

