
namespace OnlineStore.Domain.Model
{
    public class ShoppingCart : AggregateRoot
    {
        public User User { get; set; }
    }
}
