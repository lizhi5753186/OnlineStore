using System;

namespace OnlineStore.Domain.Model
{
    public class ProductCategorization : AggregateRoot
    {
        public ProductCategorization()
        { }

        public ProductCategorization(Guid productId, Guid categoryId)
        {
            this.CategoryId = categoryId;
            this.ProductId = productId;
        }

        public Guid CategoryId { get; set; }

        public Guid ProductId { get;  set; }

        public override string ToString()
        {
            return string.Format("CategoryID: {0}, ProductID: {1}", this.CategoryId, this.ProductId);
        }

        // 通过商品对象和分类对象来创建商品分类对象
        public static ProductCategorization CreateCategorization(Product product, Category category)
        {
            return new ProductCategorization(product.Id, category.Id);
        }
    }
}
