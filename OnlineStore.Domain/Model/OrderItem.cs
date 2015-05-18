using System;

namespace OnlineStore.Domain.Model
{
    public class OrderItem : IEntity
    {
        #region IEnity Member
        public Guid Id { get; set; }
        #endregion 

        public int Quantity { get; set; }
        public Product Product { get; set; }

        // 包含当前订单项的订单对象
        public Order Order { get; set; }

        public decimal ItemAmout 
        {
            get
            {
                return this.Product.UnitPrice * this.Quantity;
            }
        }

        #region Object Member
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            OrderItem other = obj as OrderItem;
            if ((object)other == null)
                return false;
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion 

        #region Public Static Operator Overrides
        public static bool operator ==(OrderItem a, OrderItem b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }
            return a.Equals(b);
        }

        public static bool operator !=(OrderItem a, OrderItem b)
        {
            return !(a == b);
        }
        #endregion 
    }
}
