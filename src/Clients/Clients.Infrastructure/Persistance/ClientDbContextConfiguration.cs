using Clients.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clients.Infrastructure.Persistance
{
    internal class ClientDbContextConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("clients");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id")
                .IsRequired();
            
            builder.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .IsRequired(true);

            builder.Property(p => p.FamilyName).HasColumnName("family_name");

            builder.Property(p => p.Created).HasColumnName("created_at");
            builder.Property(p => p.Modified).HasColumnName("updated_at");

            // Address value object mapping
            builder.OwnsOne(o => o.Address)
                .Property(p => p.City).HasColumnName("city");

            builder.OwnsOne(o => o.Address)
                .Property(p => p.Street).HasColumnName("street");

            builder.OwnsOne(o => o.Address)
                .Property(p => p.BuildingNumber).HasColumnName("building_number");

            builder.OwnsOne(o => o.Address)
                .Property(p => p.AddressLines)
                .HasColumnName("address_lines")
                .HasColumnType("jsonb");

            builder.OwnsOne(o => o.ContactDetails)
               .Property(p => p.PrimaryPhoneNumber).HasColumnName("primary_phone_number");

            builder.OwnsOne(o => o.ContactDetails)
               .Property(p => p.SecondaryPhoneNumber).HasColumnName("secondary_phone_number");

            builder.OwnsOne(o => o.ContactDetails)
               .Property(p => p.EmailAddress).HasColumnName("email_address");
        }
    }
}