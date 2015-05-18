using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineStore.Domain.Model;

namespace OnlineStore.Repositories.EntityFramework.ModelConfigurations
{
    public class OrderTypeConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderTypeConfiguration()
        {
            HasKey<Guid>(s => s.Id);
            Property(s => s.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Ignore(p => p.Subtotal);
        }
    }
}