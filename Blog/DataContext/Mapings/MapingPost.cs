using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataContext.Mapings
{
    public class MapingPost : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        { 
            builder.ToTable("Post");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

            builder.Property(x => x.LastUpdateDate)
            .IsRequired()
            .HasColumnName("LastUpdateDate")
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(x => x.Slug, "IX_Post_Slug")
            .IsUnique();

            //Identificando o relacionamento um para muitos "Um ator tem muitos posts e um post só tem um author"
            builder.HasOne(x => x.Author) // Pegando a propriedade que faz referencia ao author
            .WithMany(x => x.Posts) //Pegando a lista dos posts no author
            .HasConstraintName("FK_Post_Author") // Definindo um nome para a minha Constraint
            .OnDelete(DeleteBehavior.Cascade); // Sempre quando eu deletar um author vou deletar todos os posts relacionados com author

            //Mesma coisa com a categoria
            builder.HasOne(x => x.Category)
           .WithMany(x => x.Posts)
           .HasConstraintName("FK_Post_Category")
           .OnDelete(DeleteBehavior.Cascade);

            //Indentificando o mapeamento muitos para muitos "Um post tem muitas tags e uma tag tem muitos posts"
            builder.HasMany(j => j.Tags)
            .WithMany(j => j.Posts)
            .UsingEntity<Dictionary<string, object>>(
            "PostTag", // Nome da tabela associativa
            post => post.HasOne<Tag>()
            .WithMany()
            .HasForeignKey("PostId") //Nome do campo da tabela associativa
            .HasConstraintName("Fk_PostTag_PostId")
            .OnDelete(DeleteBehavior.Cascade),
            tag => tag.HasOne<Post>()
            .WithMany()
            .HasForeignKey("TagId")
            .HasConstraintName("FK_PostTag.TagId")
            .OnDelete(DeleteBehavior.Cascade)

            );
        }
    }
}