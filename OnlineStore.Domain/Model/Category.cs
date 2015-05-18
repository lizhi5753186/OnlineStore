
namespace OnlineStore.Domain.Model
{
    // 类别类
    public class Category : AggregateRoot
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
