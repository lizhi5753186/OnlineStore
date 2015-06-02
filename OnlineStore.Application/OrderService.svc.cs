using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OnlineStore.Domain.Model;
using OnlineStore.Infrastructure;
using OnlineStore.ServiceContracts;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class OrderService : IOrderService
   {
       private readonly IOrderService _orderServiceImp;

       public OrderService()
       {
           _orderServiceImp = ServiceLocator.Instance.GetService<IOrderService>();
       }

       public void AddProductToCart(Guid customerId, Guid productId, int quantity)
        {
           try
           {
               _orderServiceImp.AddProductToCart(customerId, productId, quantity);
           }
           catch (Exception ex)
           {
               throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
           }
        }

        public ShoppingCartDto GetShoppingCart(Guid customerId)
        {
            try
            {
                return _orderServiceImp.GetShoppingCart(customerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public int GetShoppingCartItemCount(Guid userId)
        {
            try
            {
                return _orderServiceImp.GetShoppingCartItemCount(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void UpdateShoppingCartItem(Guid shoppingCartItemId, int quantity)
        {
            try
            {
                _orderServiceImp.UpdateShoppingCartItem(shoppingCartItemId, quantity);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void DeleteShoppingCartItem(Guid shoppingCartItemId)
        {
            try
            {
                _orderServiceImp.DeleteShoppingCartItem(shoppingCartItemId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public OrderDto Checkout(Guid customerId)
        {
            try
            {
                return _orderServiceImp.Checkout(customerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }


        public OrderDto GetOrder(Guid orderId)
        {
            try
            {
                return _orderServiceImp.GetOrder(orderId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<OrderDto> GetOrdersForUser(Guid userId)
        {
            try
            {
                return _orderServiceImp.GetOrdersForUser(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<OrderDto> GetAllOrders()
        {
            try
            {
                return _orderServiceImp.GetAllOrders();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void Confirm(Guid orderId)
        {
            try
            {
                 _orderServiceImp.Confirm(orderId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void Dispatch(Guid orderId)
        {
            try
            {
                _orderServiceImp.Dispatch(orderId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }
   }
}
