using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataContext.Mapings
{
    public class MapingUser : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();


            //Name
            builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

            //Email
            builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);

            //PassWordHas
            builder.Property(x => x.PassWordHash)
            .IsRequired()
            .HasColumnName("PassWordHash")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);

            //Bio
            builder.Property(x => x.Bio)
            .IsRequired(false);

            //Image
            builder.Property(x => x.Image)
           .IsRequired(false);

            //Slug
            builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);
            builder.HasIndex(x => x.Slug, "IX_User_Slug")
           .IsUnique();

            //Mapeamento muitos para muitos 
            builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
            "UserRole",
            role => role
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey("RoleId")
            .HasConstraintName("FK_UserRole_RoleId")
            .OnDelete(DeleteBehavior.Cascade),
            user => user
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("UserId")
            .HasConstraintName("FK_UserRole_UserId")
            .OnDelete(DeleteBehavior.Cascade)
            );
        }
    }
}