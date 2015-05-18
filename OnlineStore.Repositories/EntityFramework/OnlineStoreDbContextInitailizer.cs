using System.Data.Entity;

namespace OnlineStore.Repositories.EntityFramework
{
    // DropCreateDatabaseIfModelChanges<TContext>: https://msdn.microsoft.com/zh-cn/library/gg679604(v=vs.113).aspx 
    public sealed class OnlineStoreDbContextInitailizer : DropCreateDatabaseIfModelChanges<OnlineStoreDbContext>
    {
        // 请在使用OnlineStoreDbContextInitailizer作为数据库初始化器（Database Initializer）时，去除以下代码行
        // 的注释，以便在数据库重建时，相应的SQL脚本会被执行。对于已有数据库的情况，请直接注释掉以下代码行。
        protected override void Seed(OnlineStoreDbContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_CUSTOMER_USERNAME ON Users(UserName)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_CUSTOMER_EMAIL ON Users(Email)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_LAPTOP_NAME ON Laptops(Name)");
            base.Seed(context);
        }

        public static void Initialize()
        {
            Database.SetInitializer<OnlineStoreDbContext>(null);
        }
    }
}