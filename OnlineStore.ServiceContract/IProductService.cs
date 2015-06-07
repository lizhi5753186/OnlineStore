using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.ServiceContracts.ModelDTOs;
using OnlineStore.Infrastructure.Caching;

namespace OnlineStore.ServiceContracts
{
    // 商品服务契约的定义
    [ServiceContract(Namespace="")]
    public interface IProductService
    {
        #region Methods

        [OperationContract]
        [FaultContract(typeof (FaultData))]
        // 因为创建了新商品后，缓存已失效了，所以需要移除相对应的缓存
        [Cache(CachingMethod.Remove, "GetProductsForCategory",
            "GetProductsWithPagination",
            "GetProductsForCategoryWithPagination",
            "GetNewProducts",
            "GetProducts",
            "GetProductById")]
        List<ProductDto> CreateProducts(List<ProductDto> productsDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetCategories", "GetCategoryById")]
        List<CategoryDto> CreateCategories(List<CategoryDto> categoriDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetProductsForCategory",
            "GetProductsWithPagination",
            "GetProductsForCategoryWithPagination",
            "GetNewProducts",
            "GetProducts", "GetProductById")]
        List<ProductDto> UpdateProducts(List<ProductDto> productsDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetCategories", "GetCategoryById")]
        List<CategoryDto> UpdateCategories(List<CategoryDto> categoriDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetProductsForCategory",
            "GetProductsWithPagination",
            "GetProductsForCategoryWithPagination",
            "GetNewProducts",
            "GetProducts", "GetProductById")]
        void DeleteProducts(List<string> produList);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetCategories", "GetCategoryById")]
        void DeleteCategories(List<string> categoryList);

        /// <summary>
        /// 设置商品分类
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetProductsForCategory", "GetProductsForCategoryWithPagination")]
        ProductCategorizationDto CategorizeProduct(Guid productId, Guid categoryId);

        /// <summary>
        /// 取消商品分类
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetProductsForCategory", "GetProductsForCategoryWithPagination")]
        void UncategorizeProduct(Guid productId);

        // 获得所有商品的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        IEnumerable<ProductDto> GetProducts();

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        ProductDtoWithPagination GetProductsWithPagination(Pagination pagination);


        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        IEnumerable<ProductDto> GetProductsForCategory(Guid categoryId);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        ProductDtoWithPagination GetProductsForCategoryWithPagination(Guid categoryId, Pagination pagination);

        // 获得新上市的商品的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        IEnumerable<ProductDto> GetNewProducts(int count);

        // 获得所有类别的契约方法
        [OperationContract]
        [FaultContract(typeof (FaultData))]
        [Cache(CachingMethod.Get)]
        CategoryDto GetCategoryById(Guid id);

        // 获得所有类别的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        IEnumerable<CategoryDto> GetCategories();

        // 根据商品Id来获得商品的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        ProductDto GetProductById(Guid id);

        #endregion
    }
}
