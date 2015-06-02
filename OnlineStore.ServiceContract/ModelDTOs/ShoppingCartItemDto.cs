namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class ShoppingCartItemDto
    {
        public string Id { get; set; }

        public int? Quantity { get; set; }

        public ProductDto Product { get; set; }

        public decimal? ItemAmount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}