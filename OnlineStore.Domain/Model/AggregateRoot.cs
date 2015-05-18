using System;

namespace OnlineStore.Domain.Model
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public Guid Id
        {
            get;
            set;
        }

        #region Object Member

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            IAggregateRoot ar = obj as IAggregateRoot;
            if (ar == null)
                return false;
            return this.Id == ar.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion 
    }
}
