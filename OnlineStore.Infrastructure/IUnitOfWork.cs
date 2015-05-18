namespace OnlineStore.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}