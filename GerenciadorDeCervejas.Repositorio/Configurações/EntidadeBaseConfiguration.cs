using GerenciadorDeCervejas.Infra.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorDeCervejas.Repositorio.Configurações
{
    public class EntidadeBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntidadeBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.DataCriacao)
                .HasColumnName("datacriacao")
                .IsRequired();

            builder.Property(x => x.DataAlteracao)
                .HasColumnName("dataalteracao")
                .IsRequired();
        }
    }
}
