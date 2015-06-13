using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.Domain;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Specifications;
using OnlineStore.ServiceContracts;
using OnlineStore.ServiceContracts.ModelDTOs;
using System.Linq;
using System.Transactions;
using OnlineStore.Domain.Services;
using OnlineStore.Events.Bus;

namespace OnlineStore.Application.ServiceImplementations
{
    public class OrderServiceImp : ApplicationService, IOrderService
    {
        #region Private Fileds
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDomainService _domainService;
        private readonly IEventBus _eventBus;
        #endregion 

        #region Ctor
        public OrderServiceImp(IRepositoryContext context, 
            IUserRepository userRepository, 
            IShoppingCartRepository shoppingCartRepository, 
            IProductRepository productRepository, 
            IShoppingCartItemRepository shoppingCartItemRepository, 
            IDomainService domainService, 
            IOrderRepository orderRepository, 
            IEventBus eventBus) : base(context)
        {
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _domainService = domainService;
            _orderRepository = orderRepository;
            _eventBus = eventBus;
        }

        #endregion 

        #region IOrderService Members

        public void AddProductToCart(Guid customerId, Guid productId, int quantity)
        {
            var user = _userRepository.GetByKey(customerId);

            var shoppingCart = _shoppingCartRepository.GetBySpecification(new ExpressionSpecification<ShoppingCart>(s=>s.User.Id == user.Id));
            if (shoppingCart == null)
                throw new DomainException("用户{0}不存在购物车.", customerId);

            var product = _productRepository.GetByKey(productId);
            var shoppingCartItem = _shoppingCartItemRepository.FindItem(shoppingCart, product);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    Product = product,
                    ShoopingCart = shoppingCart,
                    Quantity = quantity
                };

                _shoppingCartItemRepository.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.UpdateQuantity(shoppingCartItem.Quantity + quantity);
                _shoppingCartItemRepository.Update(shoppingCartItem);
            }

            RepositorytContext.Commit();
        }

        public ShoppingCartDto GetShoppingCart(Guid customerId)
        {
            var user = _userRepository.GetByKey(customerId);

            var shoppingCart = _shoppingCartRepository.GetBySpecification(
                new ExpressionSpecification<ShoppingCart>(s => s.User.Id == user.Id));
            if (shoppingCart == null)
                throw new DomainException("用户{0}不存在购物车.", customerId);

            var shoppingCartItems =
                _shoppingCartItemRepository.GetAll(
                    new ExpressionSpecification<ShoppingCartItem>(s => s.ShoopingCart.Id == shoppingCart.Id), elp => elp.Product);

            var shoppingCartDto = Mapper.Map<ShoppingCart, ShoppingCartDto>(shoppingCart);
            shoppingCartDto.Items = new List<ShoppingCartItemDto>();
            if (shoppingCartItems != null && shoppingCartItems.Any())
            {
                shoppingCartItems
                    .ToList()
                    .ForEach(s => shoppingCartDto.Items.Add(Mapper.Map<ShoppingCartItem, ShoppingCartItemDto>(s)));
                shoppingCartDto.Subtotal = shoppingCartDto.Items.Sum(p => p.ItemAmount);
            }

            return shoppingCartDto;
        }

        public int GetShoppingCartItemCount(Guid userId)
        {
            var user = _userRepository.GetByKey(userId);
            var shoppingCart = _shoppingCartRepository.GetBySpecification(new ExpressionSpecification<ShoppingCart>(s => s.User.Id == user.Id));
            if(shoppingCart == null)
                throw new InvalidOperationException("没有可用的购物车实例.");
            var shoppingCartItems =
                _shoppingCartItemRepository.GetAll(new ExpressionSpecification<ShoppingCartItem>(s => s.ShoopingCart.Id == shoppingCart.Id), elp => elp.Product);
            return shoppingCartItems.Sum(s => s.Quantity);
        }

        public void UpdateShoppingCartItem(Guid shoppingCartItemId, int quantity)
        {
            var shoppingCartItem = _shoppingCartItemRepository.GetByKey(shoppingCartItemId);
            shoppingCartItem.UpdateQuantity(quantity);
            _shoppingCartItemRepository.Update(shoppingCartItem);
            RepositorytContext.Commit();
        }

        public void DeleteShoppingCartItem(Guid shoppingCartItemId)
        {
            var shoppingCartItem = _shoppingCartItemRepository.GetByKey(shoppingCartItemId);
            _shoppingCartItemRepository.Remove(shoppingCartItem);
            RepositorytContext.Commit();
        }

        public OrderDto Checkout(Guid customerId)
        {
            var user = _userRepository.GetByKey(customerId);
            var shoppingCart = _shoppingCartRepository.GetByExpression(s => s.User.Id == user.Id);
            var order = _domainService.CreateOrder(user, shoppingCart);

            return Mapper.Map<Order, OrderDto>(order);
        }

        public OrderDto GetOrder(Guid orderId)
        {
            var order = _orderRepository.GetBySpecification(new ExpressionSpecification<Order>(o=>o.Id.Equals(orderId)), elp=>elp.OrderItems);
            return Mapper.Map<Order, OrderDto>(order);
        }

        // 获得指定用户的所有订单
        public IList<OrderDto> GetOrdersForUser(Guid userId)
        {
            var user = _userRepository.GetByKey(userId);
            var orders = _orderRepository.GetAll(new ExpressionSpecification<Order>(o => o.User.Id == userId), sp => sp.CreatedDate, SortOrder.Descending, elp=>elp.OrderItems);
            var orderDtos = new List<OrderDto>();
            orders
                .ToList()
                .ForEach(o=>orderDtos.Add(Mapper.Map<Order, OrderDto>(o)));
            return orderDtos;
        }

        public IList<OrderDto> GetAllOrders()
        {
            var orders = _orderRepository.GetAll(sort => sort.CreatedDate, SortOrder.Descending);
            var orderDtos = new List<OrderDto>();
            orders
                .ToList()
                .ForEach(o=>orderDtos.Add(Mapper.Map<Order, OrderDto>(o)));
            return orderDtos;
        }

        public void Confirm(Guid orderId)
        {
            using (var transactionScope = new TransactionScope())
            {
                var order = _orderRepository.GetByKey(orderId);
                order.Confirm();
                _orderRepository.Update(order);

                RepositorytContext.Commit();
                _eventBus.Commit();
                transactionScope.Complete();
            }
        }

        public void Dispatch(Guid orderId)
        {
            using (var transactionScope = new TransactionScope())
            {
                var order = _orderRepository.GetByKey(orderId);
                order.Dispatch();
                _orderRepository.Update(order);
                RepositorytContext.Commit();
                _eventBus.Commit();
                transactionScope.Complete();
            }
        }

        #endregion 
    }
}