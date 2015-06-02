using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineStore.Domain.Model;

namespace OnlineStore.Repositories.EntityFramework.ModelConfigurations
{
    public class UserRoleTypeConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleTypeConfiguration()
        {
            HasKey<Guid>(ur => ur.Id);
            Property(ur => ur.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(ur => ur.RoleId)
                .IsRequired();
            Property(ur => ur.UserId)
                .IsRequired();
        }
    }
}