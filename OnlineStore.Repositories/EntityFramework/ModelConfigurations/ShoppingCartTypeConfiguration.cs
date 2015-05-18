using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineStore.Domain.Model;

namespace OnlineStore.Repositories.EntityFramework.ModelConfigurations
{
    public class ShoppingCartTypeConfiguration : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartTypeConfiguration()
        {
            HasKey(c => c.Id);
            Property(c => c.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}