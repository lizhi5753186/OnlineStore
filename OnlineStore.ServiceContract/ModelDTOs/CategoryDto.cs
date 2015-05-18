using System.Collections.Generic;

namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class CategoryDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public IEnumerable<ProductDto> Products { get; set; }
    }
}