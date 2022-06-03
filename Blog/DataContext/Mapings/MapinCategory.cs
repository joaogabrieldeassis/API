using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataContext.Mapings
{
    public class MapinCategory : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.Id); //Indentificador 

            //Incrementar meu Id
            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();//Incrementador

            //Mapeando minha Coluna Name
            builder.Property(x => x.Name)
            .IsRequired() //Nott Null
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

            //Mapeando minha Coluna Slug
            builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

            //Indices
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique();
        }
    }
}