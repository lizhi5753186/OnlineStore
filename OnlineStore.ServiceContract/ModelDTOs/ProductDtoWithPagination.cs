using System.Collections.Generic;

namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class ProductDtoWithPagination
    {
        public Pagination Pagination { get; set; }
        public List<ProductDto> ProductDtos { get; set; }
    }
}