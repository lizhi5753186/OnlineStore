
 // ReSharper disable once CheckNamespace
namespace OnlineStore.Web.OrderService
{
   public partial class OrderDto
   {
       public string StatusText
       {
           get
           {
               if (this.Status != null)
                   switch (this.Status)
                   {
                       case OrderStatusDto.Created:
                           return "新创建";
                       case OrderStatusDto.Delivered:
                           return "已收货";
                       case OrderStatusDto.Dispatched:
                           return "已发货";
                       case OrderStatusDto.Paid:
                           return "已付款";
                       case OrderStatusDto.Picked:
                           return "已提货";
                       default:
                           return null;
                   }
               else
                   return null;
           }
       }

       public string CreatedDateText
       {
           get { return this.CreatedDate == null ? "N/A" : this.CreatedDate.Value.ToShortDateString(); }
       }

       public string DispatchedDateText
       {
           get { return this.DispatchedDate == null ? "N/A" : this.DispatchedDate.Value.ToShortDateString(); }
       }

       public string DeliveredDateText
       {
           get { return this.DeliveredDate == null ? "N/A" : this.DeliveredDate.Value.ToShortDateString(); }
       }

       public int TotalLines
       {
           get { return this.OrderItems == null ? 0 : this.OrderItems.Length; }
       }

       public string IdText
       {
           get { return this.Id.Substring(0, 14) + "..."; }
       }

       public string TotalAmount
       {
           get { return string.Format("{0:N2} 元", this.Subtotal); }
       }

       public bool CanConfirm
       {
           get
           {
               return this.Status != null && this.Status == OrderStatusDto.Dispatched;
           }
       }

       public override string ToString()
       {
           return this.Id;
       }
   }
}

namespace OnlineStore.Web.ProductService
{
    public partial class ProductDto
    {
        public string CategoryName
        {
            get
            {
                if (this.Category == null)
                    return "(未分类)";
                else
                    return this.Category.Name;
            }
        }
    }
}

