
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Domain.Model
{
    public class Order : AggregateRoot
    {
        private List<OrderItem> orderItems = new List<OrderItem>();

        public DateTime CreatedDate { get; set; }

        public DateTime? DispatchedDate { get; set; }

        public DateTime? DeliveredDate { get; set; }
        public virtual List<OrderItem> OrderItems
        { 
            get 
            {
                return orderItems; 
            }
            set
            {
                orderItems = value;
            }
        }

        public User User { get; set; }

        public Address DeliveryAddress
        { 
            get 
            { 
                return User.DeliveryAddress;
            } 
        }

        // 在严格的业务系统中，金额往往以Money模式实现。有关Money模式，请参见：http://martinfowler.com/eaaCatalog/money.html
        public decimal Subtotal
        {
            get
            {
                return this.OrderItems.Sum(p => p.ItemAmout);
            }
        }
    }
}
