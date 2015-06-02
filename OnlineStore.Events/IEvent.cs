using System;

namespace OnlineStore.Events
{
    // 事件接口
    public interface IEvent
    {
        Guid Id { get; }

        // 获取产生事件的时间
        DateTime TimeStamp { get; }
    }
}