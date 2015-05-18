using System;

namespace OnlineStore.Domain.Model
{
    public class UserRole : AggregateRoot
    {
        public Guid UserID { get; set; }

        public Guid RoleID { get; set; }
    }
}
