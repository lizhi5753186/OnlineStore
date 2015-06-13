
using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Domain.Events;

namespace OnlineStore.Domain.Model
{
    public class Order : AggregateRoot
    {
        private List<OrderItem> _orderItems = new List<OrderItem>();

        #region Public Properties
        // 获取或设置订单的状态
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 获取或设置订单的创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 获取或设置订单的发货日期
        /// </summary>
        public DateTime? DispatchedDate { get; set; }

        /// <summary>
        /// 获取或设置订单的派送日期
        /// </summary>
        public DateTime? DeliveredDate { get; set; }

        public virtual List<OrderItem> OrderItems
        { 
            get 
            {
                return _orderItems; 
            }
            set
            {
                _orderItems = value;
            }
        }

        public virtual User User { get; set; }

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

        #endregion 

        #region Ctor
        public Order()
        {
            CreatedDate = DateTime.Now;
            Status = OrderStatus.Created;
        }

        #endregion 

        #region Public Methods
        /// <summary>
        /// 当客户完成收货后，对销售订单进行确认。
        /// </summary>
        public void Confirm()
        {
            // 处理领域事件
            DomainEvent.Handle<OrderConfirmedEvent>(new OrderConfirmedEvent(this) { ConfirmedDate = DateTime.Now, OrderId = this.Id, UserEmailAddress = this.User.Email });
        }

        /// <summary>
        /// 处理发货。
        /// </summary>
        public void Dispatch()
        {
            // 处理领域事件
            DomainEvent.Handle<OrderDispatchedEvent>(new OrderDispatchedEvent(this) { DispatchedDate = DateTime.Now, OrderId = this.Id, UserEmailAddress = this.User.Email });
        }
        #endregion
    }
}
