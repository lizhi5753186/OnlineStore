using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.ServiceContracts;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application.ServiceImplementations
{
    // 商品服务的实现
    public class ProductServiceImp : ApplicationService, IProductService
    {
        #region Private Fields
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategorizationRepository _productCategorizationRepository;
        #endregion 

        #region Ctor
        public ProductServiceImp(IRepositoryContext context,
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IProductCategorizationRepository productCategorizationRepository) :base(context)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productCategorizationRepository = productCategorizationRepository;
        }

        #endregion

        #region IProductService Members
        public IEnumerable<ProductDto> GetProducts()
        {
            var result = new List<ProductDto>();

             _productRepository.
                GetAll().
                ToList().
                ForEach(p =>
                {
                    var productDto = Mapper.Map<Product, ProductDto>(p);
                    result.Add(productDto);
                });

            return result;
        }

        public IEnumerable<ProductDto> GetProductsForCategory(Guid categoryId)
        {
            var result = new List<ProductDto>();

            var category = _categoryRepository.GetByKey(categoryId);
            var products = _productCategorizationRepository.GetProductsForCategory(category);
            products.ToList().ForEach(p=>result.Add(Mapper.Map<Product, ProductDto>(p)));
            return result;
        }

        public IEnumerable<ProductDto> GetNewProducts(int count)
        {
            var newProducts = new List<ProductDto>();
            _productRepository.GetNewProducts(count)
                .ToList()
                .ForEach
                (
                    np => newProducts.Add(Mapper.Map<Product, ProductDto>(np))
                );

            return newProducts;
        }

        public CategoryDto GetCategoryById(Guid id)
        {
            var category = _categoryRepository.GetByKey(id);
            var result = Mapper.Map<Category, CategoryDto>(category);
            return result;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            var result = new List<CategoryDto>();

            _categoryRepository.GetAll().ToList().ForEach(c =>
            {
                var categoryDto = Mapper.Map<Category, CategoryDto>(c);
                result.Add(categoryDto);
            });

            return result;
        }

        public ProductDto GetProductById(Guid id)
        {
            var product = _productRepository.GetByKey(id);
            var result = Mapper.Map<Product, ProductDto>(product);
            return result;
        }

        #endregion  
    }
}