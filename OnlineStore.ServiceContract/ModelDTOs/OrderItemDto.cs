using System.Collections.Generic;

namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class OrderItemDto
    {
        public string Id { get; set; }
        public int? Quantity { get; set; }
        public string OrderId { get; set; }
        public ProductDto Product { get; set; }
        public decimal? ItemAmount { get; set; }
    }
}