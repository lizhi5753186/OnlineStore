using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineStore.Domain.Model;

namespace OnlineStore.Repositories.EntityFramework.ModelConfigurations
{
    public class UserTypeConfiguration : EntityTypeConfiguration<User>
    {
        public UserTypeConfiguration()
        {
            HasKey(c => c.Id);
            Property(c => c.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.UserName)
                .IsRequired()
                .HasMaxLength(20);
            Property(c => c.Password)
                .IsRequired()
                .HasMaxLength(20);
            Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(80);
            // 不加下面的配置，将会出现下面的错误，更多解决方案参考：http://www.tuicool.com/articles/M7RbAj
            // The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value.
            Property(c => c.RegisteredDate)
                .HasColumnType("datetime2")
                .HasPrecision(0);

            ToTable("Users");
        }
    }
}