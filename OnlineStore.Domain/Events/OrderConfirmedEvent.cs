using System;

namespace OnlineStore.Domain.Events
{
    [Serializable]
    public class OrderConfirmedEvent : DomainEvent
    {
        #region Ctor
        public OrderConfirmedEvent() { }
        public OrderConfirmedEvent(IEntity source) : base(source) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置订单确认的日期。
        /// </summary>
        public DateTime ConfirmedDate { get; set; }
        public string UserEmailAddress { get; set; }
        public Guid OrderId { get; set; }
        #endregion 
    }
}