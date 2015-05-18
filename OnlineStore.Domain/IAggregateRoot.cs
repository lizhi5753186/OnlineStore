using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain
{
    // 聚合根接口，继承于该接口的对象是外部唯一操作的对象
    public interface IAggregateRoot : IEntity
    {
    }
}
