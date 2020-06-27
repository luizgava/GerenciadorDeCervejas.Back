using GerenciadorDeCervejas.Dominio.Cervejas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorDeCervejas.Repositorio.Configurações
{
    public class CervejaConfiguration : EntidadeBaseConfiguration<Cerveja>
    {
        public override void Configure(EntityTypeBuilder<Cerveja> builder)
        {
            builder.ToTable("cervejas");

            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.Harmonizacao)
                .HasColumnName("harmonizacao")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Cor)
                .HasColumnName("cor")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.TeorAlcoolico)
                .HasColumnName("teoralcoolico")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Temperatura)
                .HasColumnName("temperatura")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Ingredientes)
                .HasColumnName("ingredientes")
                .HasMaxLength(1000)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
