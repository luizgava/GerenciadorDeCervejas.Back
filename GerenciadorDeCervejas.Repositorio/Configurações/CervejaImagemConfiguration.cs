using System.Runtime.ConstrainedExecution;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas.Imagem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorDeCervejas.Repositorio.Configurações
{
    public class CervejaImagemConfiguration : EntidadeBaseConfiguration<CervejaImagem>
    {
        public override void Configure(EntityTypeBuilder<CervejaImagem> builder)
        {
            base.Configure(builder);

            builder.ToTable("cervejas_imagens");

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.NomeArquivo)
                .HasColumnName("nomearquivo")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ContentType)
                .HasColumnName("contenttype")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Bytes)
                .HasColumnName("bytes")
                .IsRequired();

            builder.HasOne(x => x.Cerveja)
                .WithOne()
                .HasForeignKey<CervejaImagem>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
