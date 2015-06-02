using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.Domain;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.ServiceContracts.ModelDTOs;
using OrderStatus = OnlineStore.Domain.Model.OrderStatus;

namespace OnlineStore.Application
{
    // 定义一个应用服务抽象类，以便把重复的代码放在该抽象类中
    public abstract class ApplicationService
    {
        private readonly IRepositoryContext _repositoryContext;

        protected ApplicationService(IRepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        
        protected IRepositoryContext RepositorytContext 
        {
            get { return this._repositoryContext; }
        }

        #region Protected Methods

        // 判断给定字符串是否是Guid.Empty
        protected bool IsEmptyGuidString(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return true;
            var guid = new Guid(s);
            return guid == Guid.Empty;
        }

       
        // 处理简单的聚合创建逻辑。
        protected TDtoList PerformCreateObjects<TDtoList, TDto, TAggregateRoot>(TDtoList dataTransferObjects,
            IRepository<TAggregateRoot> repository,
            Action<TDto> processDto = null,
            Action<TAggregateRoot> processAggregateRoot = null)
            where TDtoList : List<TDto>, new() where TAggregateRoot : class, IAggregateRoot
        {
            if (dataTransferObjects == null)
                throw new ArgumentNullException("dataTransferObjects");
            if (repository == null)
                throw new ArgumentNullException("repository");
            TDtoList result = new TDtoList();
            if (dataTransferObjects.Count <= 0) return result;
            var ars = new List<TAggregateRoot>();
            
            foreach (var dto in dataTransferObjects)
            {
                if (processDto != null)
                    processDto(dto);
                var ar = Mapper.Map<TDto, TAggregateRoot>(dto);
                if (processAggregateRoot != null)
                    processAggregateRoot(ar);
                ars.Add(ar);
                repository.Add(ar);
            }

            RepositorytContext.Commit();
            ars.ForEach(ar => result.Add(Mapper.Map<TAggregateRoot, TDto>(ar)));
            return result;
        }

        // 处理简单的聚合更新操作。
        protected TDtoList PerformUpdateObjects<TDtoList, TDataObject, TAggregateRoot>(TDtoList dataTransferObjects,
            IRepository<TAggregateRoot> repository,
            Func<TDataObject, string> idFunc,
            Action<TAggregateRoot, TDataObject> updateAction)
            where TDtoList : List<TDataObject>, new()
            where TAggregateRoot : class, IAggregateRoot
        {
            if (dataTransferObjects == null)
                throw new ArgumentNullException("dataTransferObjects");
            if (repository == null)
                throw new ArgumentNullException("repository");
            if (idFunc == null)
                throw new ArgumentNullException("idFunc");
            if (updateAction == null)
                throw new ArgumentNullException("updateAction");
            TDtoList result = null;
            if (dataTransferObjects.Count > 0)
            {
                result = new TDtoList();
                foreach (var dto in dataTransferObjects)
                {
                    if (IsEmptyGuidString(idFunc(dto)))
                        throw new ArgumentNullException("Id");
                    var id = new Guid(idFunc(dto));
                    var ar = repository.GetByKey(id);
                    updateAction(ar, dto);
                    repository.Update(ar);
                    result.Add(Mapper.Map<TAggregateRoot, TDataObject>(ar));
                }

                RepositorytContext.Commit();
            }
            return result;
        }

        // 处理简单的删除聚合根的操作。
        protected void PerformDeleteObjects<TAggregateRoot>(IList<string> ids, IRepository<TAggregateRoot> repository, Action<Guid> preDelete = null, Action<Guid> postDelete = null)
            where TAggregateRoot : class, IAggregateRoot
        {
            if (ids == null)
                throw new ArgumentNullException("ids");
            if (repository == null)
                throw new ArgumentNullException("repository");
            foreach (var id in ids)
            {
                var guid = new Guid(id);
                if (preDelete != null)
                    preDelete(guid);
                var ar = repository.GetByKey(guid);
                repository.Remove(ar);
                if (postDelete != null)
                    postDelete(guid);
            }

            RepositorytContext.Commit();
        }

        #endregion 

        // AutoMapper框架的初始化
        public static void Initialize()
        {
            Mapper.CreateMap<AddressDto, Address>();
            Mapper.CreateMap<UserDto, User>()
                .ForMember(uMermber => uMermber.ContactAddress, mceUto=> mceUto.ResolveUsing<AddressResolver>().FromMember(fm=>fm.ContactAddress))
                .ForMember(uMember => uMember.DeliveryAddress, mceUto =>
                        mceUto.ResolveUsing<AddressResolver>().FromMember(fm => fm.DeliveryAddress));

            Mapper.CreateMap<User, UserDto>()
               .ForMember(udoMember => udoMember.ContactAddress, mceU =>
                   mceU.ResolveUsing<InversedAddressResolver>().FromMember(fm => fm.ContactAddress))
                   .ForMember(udoMember => udoMember.DeliveryAddress, mceU =>
                       mceU.ResolveUsing<InversedAddressResolver>().FromMember(fm => fm.DeliveryAddress));

            Mapper.CreateMap<Product, ProductDto>();
            Mapper.CreateMap<ProductDto, Product>();
            Mapper.CreateMap<Category, CategoryDto>();
            Mapper.CreateMap<CategoryDto, Category>();
            Mapper.CreateMap<ShoppingCart, ShoppingCartDto>();
            Mapper.CreateMap<ShoppingCartDto, ShoppingCart>();
            Mapper.CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
            Mapper.CreateMap<ShoppingCartItemDto, ShoppingCartItem>();
            Mapper.CreateMap<OrderItem, OrderItemDto>();
            Mapper.CreateMap<OrderItemDto, OrderItem>();
            Mapper.CreateMap<Order, OrderDto>()
                //.ForMember(odtoMember => odtoMember.Subtotal,
                //    mceO => mceO.ResolveUsing(
                //        o => o.OrderItems.Sum(item => item.ItemAmout)))
                .ForMember(odtoMember => odtoMember.UserContact,
                    mceO => mceO.ResolveUsing(o => o.User.Contact))
                .ForMember(odtoMember => odtoMember.UserPhone,
                    mceO => mceO.ResolveUsing(o => o.User.PhoneNumber))
                .ForMember(odtoMember => odtoMember.UserEmail,
                    mceO => mceO.ResolveUsing(o => o.User.Email))
                .ForMember(odtoMember => odtoMember.UserId,
                    mceO => mceO.ResolveUsing(o => o.User.Id))
                .ForMember(odtoMember => odtoMember.UserName,
                    mceO => mceO.ResolveUsing(o => o.User.UserName))
                .ForMember(odtoMember => odtoMember.UserAddressCountry,
                    mceO => mceO.ResolveUsing(o => o.User.DeliveryAddress.Country))
                .ForMember(odtoMember => odtoMember.UserAddressState,
                    mceO => mceO.ResolveUsing(o => o.User.DeliveryAddress.State))
                .ForMember(odtoMember => odtoMember.UserAddressCity,
                    mceO => mceO.ResolveUsing(o => o.User.DeliveryAddress.City))
                .ForMember(odtoMember => odtoMember.UserAddressStreet,
                    mceO => mceO.ResolveUsing(o => o.User.DeliveryAddress.Street))
                .ForMember(odtoMember => odtoMember.UserAddressZip,
                    mceO => mceO.ResolveUsing(o => o.User.DeliveryAddress.Zip))
                .ForMember(odtoMember => odtoMember.Status,
                    mceO => mceO.ResolveUsing(o =>
                    {
                        switch (o.Status)
                        {
                            case OrderStatus.Created:
                                return OrderStatusDto.Created;
                            case OrderStatus.Delivered:
                                return OrderStatusDto.Delivered;
                            case OrderStatus.Dispatched:
                                return OrderStatusDto.Dispatched;
                            case OrderStatus.Paid:
                                return OrderStatusDto.Paid;
                            case OrderStatus.Picked:
                                return OrderStatusDto.Picked;
                            default:
                                throw new InvalidOperationException();
                        }
                    }));
            Mapper.CreateMap<OrderDto, Order>();
            Mapper.CreateMap<ProductCategorization, ProductCategorizationDto>();
            Mapper.CreateMap<ProductCategorizationDto, ProductCategorization>();
            Mapper.CreateMap<Role, RoleDto>();
            Mapper.CreateMap<RoleDto, Role>();
        }

    }
}