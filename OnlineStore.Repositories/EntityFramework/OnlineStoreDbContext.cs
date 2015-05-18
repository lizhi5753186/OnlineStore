using System.Data.Entity;
using OnlineStore.Domain.Model;
using OnlineStore.Repositories.EntityFramework.ModelConfigurations;

namespace OnlineStore.Repositories.EntityFramework
{
    public sealed class OnlineStoreDbContext : DbContext
    {
        #region Ctor
        public OnlineStoreDbContext()
            : base("OnlineStore")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }
        #endregion 

        #region Public Properties

        public DbSet<Product> Products 
        { 
            get { return this.Set<Product>(); }
        }

        public DbSet<Category> Categories
        {
            get { return this.Set<Category>(); }
        }

        public DbSet<User> Users
        {
            get { return Set<User>(); }
        }

        public DbSet<ShoppingCart> ShoppingCarts
        {
            get { return Set<ShoppingCart>(); }
        }

        public DbSet<ProductCategorization> ProductCategorizations
        {
            get { return Set<ProductCategorization>(); }
        }

        public DbSet<UserRole> UserRoles
        {
            get { return Set<UserRole>(); }
        }

        public DbSet<Role> Roles
        {
            get { return Set<Role>(); }
        }

        public DbSet<Order> Orders
        {
            get { return Set<Order>(); }
        }
        #endregion

        #region Protected Methods
        // 
        // http://stackoverflow.com/questions/5270721/using-guid-as-pk-with-ef4-code-first
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Configurations
                .Add(new UserTypeConfiguration())
                .Add(new ProductTypeConfiguration())
                .Add(new CategoryTypeConfiguration())
                .Add(new ProductCategorizationTypeConfiguration())
                .Add(new OrderItemTypeConfiguration())
                .Add(new OrderTypeConfiguration())
                .Add(new ShoppingCartItemTypeConfiguration())
                .Add(new ShoppingCartTypeConfiguration())
                .Add(new RoleTypeConfiguration())
                .Add(new UserRoleTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
