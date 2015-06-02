namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class ProductDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? UnitPrice { get; set; }

        public string ImageUrl { get; set; }

        public bool? IsNew { get; set; }

        public CategoryDto Category { get; set; }
    }
}