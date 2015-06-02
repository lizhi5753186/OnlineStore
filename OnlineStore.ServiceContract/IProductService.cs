using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.ServiceContracts
{
    // 商品服务契约的定义
    [ServiceContract(Namespace="")]
    public interface IProductService
    {
        #region Methods

        [OperationContract]
        [FaultContract(typeof (FaultData))]
        List<ProductDto> CreateProducts(List<ProductDto> productsDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<CategoryDto> CreateCategories(List<CategoryDto> categoriDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<ProductDto> UpdateProducts(List<ProductDto> productsDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        List<CategoryDto> UpdateCategories(List<CategoryDto> categoriDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteProducts(List<string> produList);


        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteCategories(List<string> categoryList);

        /// <summary>
        /// 设置商品分类
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        ProductCategorizationDto CategorizeProduct(Guid productId, Guid categoryId);

        /// <summary>
        /// 取消商品分类
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UncategorizeProduct(Guid productId);

            // 获得所有商品的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IEnumerable<ProductDto> GetProducts();

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IEnumerable<ProductDto> GetProductsForCategory(Guid categoryId);


        // 获得新上市的商品的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IEnumerable<ProductDto> GetNewProducts(int count);

        // 获得所有类别的契约方法
        [OperationContract]
        [FaultContract(typeof (FaultData))]
        CategoryDto GetCategoryById(Guid id);

        // 获得所有类别的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IEnumerable<CategoryDto> GetCategories();

        // 根据商品Id来获得商品的契约方法
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        ProductDto GetProductById(Guid id);


        #endregion
    }
}
