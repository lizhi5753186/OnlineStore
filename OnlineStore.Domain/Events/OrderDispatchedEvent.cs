using System;

namespace OnlineStore.Domain.Events
{
    [Serializable]
    public class OrderDispatchedEvent : DomainEvent
    {
        #region Ctor
        public OrderDispatchedEvent() { }
        public OrderDispatchedEvent(IEntity source) : base(source) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置订单发货的日期。
        /// </summary>
        public DateTime DispatchedDate { get; set; }
        public string UserEmailAddress { get; set; }
        public Guid OrderId { get; set; }
        #endregion
    }
}