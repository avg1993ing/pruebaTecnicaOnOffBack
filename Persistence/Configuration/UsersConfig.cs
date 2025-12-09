using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class UsersConfig : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(e => e.id);
            builder.ToTable("users");
            builder.Property(e => e.id).HasColumnName("idusers");
            builder.Property(e => e.NameUser).HasColumnName("nameuser");
            builder.Property(e => e.Email).HasColumnName("email");
            builder.Property(e => e.IsActive).HasColumnName("isactive");
            builder.Property(e => e.PasswordUser).HasColumnName("passworduser");

        }
    }
}

