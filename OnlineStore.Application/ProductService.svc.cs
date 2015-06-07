using System;
using System.Collections.Generic;
using System.ServiceModel;
using OnlineStore.Infrastructure;
using OnlineStore.ServiceContracts;
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
            }
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

        public List<ProductDto> CreateProducts(List<ProductDto> productsDtos)
        {
            try
            {
                return _productService.CreateProducts(productsDtos);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public List<CategoryDto> CreateCategories(List<CategoryDto> categoriDtos)
        {
            try
            {
                return _productService.CreateCategories(categoriDtos);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public List<ProductDto> UpdateProducts(List<ProductDto> productsDtos)
        {
            try
            {
                return _productService.UpdateProducts(productsDtos);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public List<CategoryDto> UpdateCategories(List<CategoryDto> categoriDtos)
        {
            try
            {
                return _productService.UpdateCategories(categoriDtos);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void DeleteProducts(List<string> produtList)
        {
            try
            {
                _productService.DeleteProducts(produtList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void DeleteCategories(List<string> categoryList)
        {
            try
            {
                _productService.DeleteCategories(categoryList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public ProductCategorizationDto CategorizeProduct(Guid productId, Guid categoryId)
        {
            try
            {
                return _productService.CategorizeProduct(productId, categoryId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void UncategorizeProduct(Guid productId)
        {
            try
            {
                _productService.UncategorizeProduct(productId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public ProductDtoWithPagination GetProductsWithPagination(Pagination pagination)
        {
            try
            {
                return _productService.GetProductsWithPagination(pagination);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public ProductDtoWithPagination GetProductsForCategoryWithPagination(Guid categoryId, Pagination pagination)
        {
            try
            {
                return _productService.GetProductsForCategoryWithPagination(categoryId, pagination);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }
    }
}