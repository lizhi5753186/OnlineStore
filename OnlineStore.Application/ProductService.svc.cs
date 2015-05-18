using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OnlineStore.ServiceContracts;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Infrastructure;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application
{
    // 商品WCF服务
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ProductService : IProductService
    {
        // 引用商品服务接口
        private readonly IProductService _productService;

        public ProductService()
        {
            _productService = ServiceLocator.Instance.GetService<IProductService>();
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            try
            {
                return _productService.GetProducts();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IEnumerable<ProductDto> GetProductsForCategory(Guid categoryId)
        {
            try
            {
                return _productService.GetProductsForCategory(categoryId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IEnumerable<ProductDto> GetNewProducts(int count)
        {
            try
            {
                return _productService.GetNewProducts(count);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public CategoryDto GetCategoryById(Guid id)
        {
            try
            {
                return _productService.GetCategoryById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            };
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            try
            {
                return _productService.GetCategories();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public ProductDto GetProductById(Guid id)
        {
            try
            {
                return _productService.GetProductById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }


       
    }
}
