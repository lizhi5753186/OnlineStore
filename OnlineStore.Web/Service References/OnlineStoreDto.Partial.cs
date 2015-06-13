
 // ReSharper disable once CheckNamespace

using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "请输入商品名称", AllowEmptyStrings = false)]
        [Display(Name = "商品名称")]
        public string NameText { get; set; }

        [Required(ErrorMessage = "请输入商品说明", AllowEmptyStrings = false)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "商品说明")]
        public string DescriptionText { get; set; }

        [Required(ErrorMessage = "请选择商品图片", AllowEmptyStrings = false)]
        [Display(Name = "商品图片")]
        public string ImageUrlText { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "输入的数据必须是货币类型")]
        [Required(ErrorMessage = "请输入单价", AllowEmptyStrings = false)]
        [Display(Name = "单价")]
        public decimal? UnitPriceText { get; set; }

        [Display(Name = "是否为新商品？")]
        [Required(ErrorMessage = "请设置该商品是否为新商品")]
        public bool? IsNewText { get; set; } 
    }

    public  partial class CategoryDto
    {
        [Required(ErrorMessage = "请输入商品分类名称", AllowEmptyStrings = false)]
        [Display(Name = "分类名称")]
        public string NameText { get; set; }

        [Required(ErrorMessage = "请输入商品分类说明", AllowEmptyStrings = false)]
        [Display(Name = "分类说明")]
        [DataType(DataType.MultilineText)]
        public string DescriptionText { get; set; } 
    }
}

