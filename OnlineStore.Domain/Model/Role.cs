
namespace OnlineStore.Domain.Model
{
    public class Role : AggregateRoot
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
