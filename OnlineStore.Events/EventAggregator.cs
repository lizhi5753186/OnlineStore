using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OnlineStore.Infrastructure;

namespace OnlineStore.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly object _sync = new object();
        private readonly Dictionary<Type, List<object>> _eventHandlers = new Dictionary<Type, List<object>>();
        private readonly MethodInfo _registerEventHandlerMethod;

        public EventAggregator()
        {
            
            // 通过反射获得EventAggregator的Register方法 
            _registerEventHandlerMethod = (from p in this.GetType().GetMethods()
                                          let methodName = p.Name
                                          let parameters = p.GetParameters()
                                          where methodName == "Register" &&
                                          parameters != null &&
                                          parameters.Length == 1 &&
                                          parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                                          select p).First();
        }

        public EventAggregator(object[] handlers)
            : this()
        {
            // 遍历注册的EventHandler来把配置文件中具体的EventHanler通过Register添加进_eventHandlers字典中
            foreach (var obj in handlers)
            {
                var type = obj.GetType();
                var implementedInterfaces = type.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType &&
                        implementedInterface.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    {
                        var eventType = implementedInterface.GetGenericArguments().First();
                        var method = _registerEventHandlerMethod.MakeGenericMethod(eventType);
                        // 调用Register方法将EventHandler添加进_eventHandlers字典中
                        method.Invoke(this, new object[] { obj });
                    }
                }
            }
        }

        public void Register<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (_sync)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null)
                    {
                        handlers.Add(eventHandler);
                    }
                    else
                    {
                        handlers = new List<object> {eventHandler};
                    }
                }
                else
                    _eventHandlers.Add(eventType, new List<object> { eventHandler });
            }
        }

        public void Register<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Register<TEvent>(eventHandler);
        }

        // 调用具体的EventHanler的Handle方法来对事件进行处理
         public void Handle<TEvent>(TEvent evnt)
            where TEvent : class, IEvent
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");
            var eventType = evnt.GetType();
            if (_eventHandlers.ContainsKey(eventType) &&
                _eventHandlers[eventType] != null &&
                _eventHandlers[eventType].Count > 0)
            {
                var handlers = _eventHandlers[eventType];
                foreach (var handler in handlers)
                {
                    var eventHandler = handler as IEventHandler<TEvent>;
                    if(eventHandler == null)
                        continue;

                    // 异步处理
                    if (eventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                    {
                        Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), evnt);
                    }
                    else
                    {
                        eventHandler.Handle(evnt);
                    }
                }
            }
        }

        public void Handle<TEvent>(TEvent evnt, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");
            var eventType = evnt.GetType();
            if (_eventHandlers.ContainsKey(eventType) &&
                _eventHandlers[eventType] != null &&
                _eventHandlers[eventType].Count > 0)
            {
                var handlers = _eventHandlers[eventType];
                List<Task> tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        if (eventHandler == null)
                            continue;
                        if (eventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), evnt));
                        }
                        else
                        {
                            eventHandler.Handle(evnt);
                        }
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(evnt, true, null);
                }
                catch (Exception ex)
                {
                    callback(evnt, false, ex);
                }
            }
            else
                callback(evnt, false, null);
        }
    }
}