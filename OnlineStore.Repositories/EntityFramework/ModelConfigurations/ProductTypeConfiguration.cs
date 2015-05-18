using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineStore.Domain.Model;

namespace OnlineStore.Repositories.EntityFramework.ModelConfigurations
{
    public class ProductTypeConfiguration : EntityTypeConfiguration<Product>
    {
         #region Ctor
        
        public ProductTypeConfiguration()
        {
            HasKey<Guid>(l => l.Id);
            Property(p => p.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Description)
                .IsRequired();
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(40);
            Property(p => p.ImageUrl)
                .HasMaxLength(255);
        }
        #endregion
    }
}