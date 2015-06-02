using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Model
{
    public class ShoppingCartItem : AggregateRoot
    {
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual ShoppingCart ShoopingCart { get; set; }

        public decimal ItemAmount
        {
            get
            {
                return this.Product.UnitPrice * this.Quantity;
            }
        }

        #region  Public Methods

        // 将当前的购物车中的项目转换为订单项
        public OrderItem ConvertToOrderItem()
        {
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(), 
                Product = this.Product, 
                Quantity = this.Quantity
            };
            return orderItem;
        }

        public void UpdateQuantity(int quantity)
        {
            this.Quantity = quantity;
        }
        #endregion 
    }
}
