using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistance.Configurations
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable(nameof(Cliente));
            
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Apellidos)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.FechaNacimiento)
                .IsRequired();

            builder.Property(p => p.Telefono)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(100);

            builder.Property(p => p.Direccion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Edad);

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(30);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(30);

        }
    }
}
