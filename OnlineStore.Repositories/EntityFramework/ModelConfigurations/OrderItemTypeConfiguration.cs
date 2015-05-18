using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineStore.Domain.Model;

namespace OnlineStore.Repositories.EntityFramework.ModelConfigurations
{
    public class OrderItemTypeConfiguration : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemTypeConfiguration()
        {
            HasKey<Guid>(s => s.Id);
            Property(p => p.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(p => p.Order)
                .WithMany(p => p.OrderItems);
            Ignore(p => p.ItemAmout);
        }
    }
}