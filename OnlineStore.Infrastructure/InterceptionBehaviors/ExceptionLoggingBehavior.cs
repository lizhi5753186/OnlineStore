using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace OnlineStore.Infrastructure.InterceptionBehaviors
{
    // 用于异常日志记录的拦截行为
    public class ExceptionLoggingBehavior :IInterceptionBehavior
    {
        /// <summary>
        /// 需要拦截的对象类型的接口
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// 通过该方法来拦截调用并执行所需要的拦截行为
        /// </summary>
        /// <param name="input">调用拦截目标时的输入信息</param>
        /// <param name="getNext">通过行为链来获取下一个拦截行为的委托</param>
        /// <returns>从拦截目标获得的返回信息</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // 执行目标方法
            var methodReturn = getNext().Invoke(input, getNext);
            // 方法执行后的处理
            if (methodReturn.Exception != null)
            {
                Utils.Log(methodReturn.Exception);
            }

            return methodReturn;
        }

        // 表示当拦截行为被调用时，是否需要执行某些操作
        public bool WillExecute
        {
            get { return true; }
        }
    }
}