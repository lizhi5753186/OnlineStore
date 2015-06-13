using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using AutoMapper;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.Domain.Services;
using OnlineStore.Domain.Specifications;
using OnlineStore.ServiceContracts;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application.ServiceImplementations
{
    public class UserServiceImp :ApplicationService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDomainService _domainService;

        public UserServiceImp(IRepositoryContext repositoryContext, 
            IUserRepository userRepository, 
            IShoppingCartRepository shoppingCartRepository, 
            IDomainService domainService, 
            IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository)
            : base(repositoryContext)
        {
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _domainService = domainService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        #region IUserService Members
        public IList<UserDto> CreateUsers(List<UserDto> userDtos)
        {
            if (userDtos == null)
                throw new ArgumentNullException("userDtos");
            return PerformCreateObjects<List<UserDto>, UserDto, User>(userDtos,
                _userRepository,
                dto =>
                {
                    if (dto.RegisteredDate == null)
                        dto.RegisteredDate = DateTime.Now;
                },
                ar =>
                {
                    var shoppingCart = ar.CreateShoppingCart();
                    _shoppingCartRepository.Add(shoppingCart);
                });
        }

        public bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return _userRepository.CheckPassword(userName, password);
        }

        public bool DisableUser(UserDto userDto)
        {
            if(userDto == null)
                throw new ArgumentNullException("userDto");
            User user;
            if (!IsEmptyGuidString(userDto.Id))
                user = _userRepository.GetByKey(new Guid(userDto.Id));
            else if (!string.IsNullOrEmpty(userDto.UserName))
                user = _userRepository.GetByExpression(u=>u.UserName == userDto.UserName);
            else if (!string.IsNullOrEmpty(userDto.Email))
                user = _userRepository.GetByExpression(u => u.Email == userDto.Email);
            else
                throw new ArgumentNullException("userDto", "Either ID, UserName or Email should be specified.");
            user.Disable();
            _userRepository.Update(user);
            RepositorytContext.Commit();
            return user.IsDisabled;
        }

        public bool EnableUser(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException("userDto");
            User user;
            if (!IsEmptyGuidString(userDto.Id))
                user = _userRepository.GetByKey(new Guid(userDto.Id));
            else if (!string.IsNullOrEmpty(userDto.UserName))
                user = _userRepository.GetByExpression(u => u.UserName == userDto.UserName);
            else if (!string.IsNullOrEmpty(userDto.Email))
                user = _userRepository.GetByExpression(u => u.Email == userDto.Email);
            else
                throw new ArgumentNullException("userDto", "Either ID, UserName or Email should be specified.");
            user.Enable();
            _userRepository.Update(user);
            RepositorytContext.Commit();
            return user.IsDisabled;
        }

        public IList<UserDto> UpdateUsers(List<UserDto> userDataObjects)
        {
            return PerformUpdateObjects<List<UserDto>, UserDto, User>(userDataObjects, _userRepository,
                userDto => userDto.Id,
                (u, userDto) =>
                {
                    if (!string.IsNullOrEmpty(userDto.Contact))
                        u.Contact = userDto.Contact;
                    if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                        u.PhoneNumber = userDto.PhoneNumber;
                    if (userDto.ContactAddress != null)
                    {
                        if (!string.IsNullOrEmpty(userDto.ContactAddress.City))
                            u.ContactAddress.City = userDto.ContactAddress.City;
                        if (!string.IsNullOrEmpty(userDto.ContactAddress.Country))
                            u.ContactAddress.Country = userDto.ContactAddress.Country;
                        if (!string.IsNullOrEmpty(userDto.ContactAddress.State))
                            u.ContactAddress.State = userDto.ContactAddress.State;
                        if (!string.IsNullOrEmpty(userDto.ContactAddress.Street))
                            u.ContactAddress.Street = userDto.ContactAddress.Street;
                        if (!string.IsNullOrEmpty(userDto.ContactAddress.Zip))
                            u.ContactAddress.Zip = userDto.ContactAddress.Zip;
                    }
                    if (userDto.DeliveryAddress != null)
                    {
                        if (!string.IsNullOrEmpty(userDto.DeliveryAddress.City))
                            u.DeliveryAddress.City = userDto.DeliveryAddress.City;
                        if (!string.IsNullOrEmpty(userDto.DeliveryAddress.Country))
                            u.DeliveryAddress.Country = userDto.DeliveryAddress.Country;
                        if (!string.IsNullOrEmpty(userDto.DeliveryAddress.State))
                            u.DeliveryAddress.State = userDto.DeliveryAddress.State;
                        if (!string.IsNullOrEmpty(userDto.DeliveryAddress.Street))
                            u.DeliveryAddress.Street = userDto.DeliveryAddress.Street;
                        if (!string.IsNullOrEmpty(userDto.DeliveryAddress.Zip))
                            u.DeliveryAddress.Zip = userDto.DeliveryAddress.Zip;
                    }
                    if (userDto.LastLogonDate != null)
                        u.LastLogonDate = userDto.LastLogonDate;
                    if (userDto.RegisteredDate != null)
                        u.RegisteredDate = userDto.RegisteredDate.Value;
                    if (!string.IsNullOrEmpty(userDto.Email))
                        u.Email = userDto.Email;

                    if (userDto.IsDisabled != null)
                    {
                        if (userDto.IsDisabled.Value)
                            u.Disable();
                        else
                            u.Enable();
                    }

                    if (!string.IsNullOrEmpty(userDto.Password))
                        u.Password = userDto.Password;
                });
        }

        public void DeleteUsers(List<UserDto> userDtos)
        {
            if (userDtos == null)
                throw new ArgumentNullException("userDtos");
            foreach (var userDto in userDtos)
            {
                User user = null;
                if (!IsEmptyGuidString(userDto.Id))
                    user = _userRepository.GetByKey(new Guid(userDto.Id));
                else if (!string.IsNullOrEmpty(userDto.UserName))
                    user = _userRepository.GetByExpression(u => u.UserName == userDto.UserName);
                else if (!string.IsNullOrEmpty(userDto.Email))
                    user = _userRepository.GetByExpression(u=>u.Email == userDto.Email);
                else
                    throw new ArgumentNullException("userDtos", "Either ID, UserName or Email should be specified.");
                var userRole = _userRoleRepository.GetBySpecification(Specification<UserRole>.Eval(ur => ur.UserId == user.Id));
                if (userRole != null)
                    _userRoleRepository.Remove(userRole);
                _userRepository.Remove(user);
            }

            RepositorytContext.Commit();
        }

        public UserDto GetUserByKey(Guid id)
        {
            var user = _userRepository.GetByKey(id);
            var userDto = Mapper.Map<User, UserDto>(user);
            return userDto;
        }

        public UserDto GetUserByEmail(string email)
        {
            if(string.IsNullOrEmpty(email))
                throw new ArgumentException("email");
            var user = _userRepository.GetByExpression(u => u.Email == email);
            var userDto = Mapper.Map<User, UserDto>(user);
            return userDto;
        }

        public UserDto GetUserByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("userName");
            var user = _userRepository.GetByExpression(u => u.UserName == userName);
            var userDto = Mapper.Map<User, UserDto>(user);
            return userDto;
        }


        public IList<UserDto> GetUsers()
        {
            var users = _userRepository.GetAll();
            if (users == null)
                return null;
            var result = new List<UserDto>();
            foreach (var user in users)
            {
                var userDto = Mapper.Map<User, UserDto>(user);
                var role = _userRoleRepository.GetRoleForUser(user);
                if (role != null)
                {
                    userDto.Role = Mapper.Map<Role, RoleDto>(role);
                }

                result.Add(userDto);
            }

            return result;
        }

        public IList<RoleDto> GetRoles()
        {
            var roles = _roleRepository.GetAll();
            if (roles == null)
                return null;
            var result = roles.Select(role => Mapper.Map<Role, RoleDto>(role)).ToList();
            return result;
        }

        public RoleDto GetRoleByKey(Guid id)
        {
            return Mapper.Map<Role, RoleDto>(_roleRepository.GetByKey(id));
        }

        public IList<RoleDto> CreateRoles(List<RoleDto> roleDataObjects)
        {
            return PerformCreateObjects<List<RoleDto>, RoleDto, Role>(roleDataObjects, _roleRepository);
        }

        public IList<RoleDto> UpdateRoles(List<RoleDto> roleDataObjects)
        {
            return PerformUpdateObjects<List<RoleDto>, RoleDto, Role>(roleDataObjects,
                _roleRepository,
                roleDto => roleDto.Id,
                (r, roleDto) =>
                {
                    if (!string.IsNullOrEmpty(roleDto.Name))
                        r.Name = roleDto.Name;
                    if (!string.IsNullOrEmpty(roleDto.Description))
                        r.Description = roleDto.Description;
                });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleList">需要删除的角色ID值列表</param>
        public void DeleteRoles(List<string> roleList)
        {
            PerformDeleteObjects<Role>(roleList,
                _roleRepository,
                id =>
                {
                    var userRole = _userRoleRepository.GetBySpecification(Specification<UserRole>.Eval(ur => ur.RoleId == id));
                    if (userRole != null)
                        _userRoleRepository.Remove(userRole);
                });
        }

        public void AssignRole(Guid userId, Guid roleId)
        {
            var user = _userRepository.GetByKey(userId);
            var role = _roleRepository.GetByKey(roleId);
            _domainService.AssignRole(user, role);
        }

        public void UnassignRole(Guid userId)
        {
            var user = _userRepository.GetByKey(userId);
            _domainService.UnassignRole(user);
        }

        // 根据指定的用户名，获取该用户所属的角色
        public RoleDto GetRoleByUserName(string userName)
        {
            var user = _userRepository.GetByExpression(u=>u.UserName == userName);
            var role = _userRoleRepository.GetRoleForUser(user);
            return Mapper.Map<Role, RoleDto>(role);
        }

        public IList<OrderDto> GetOrdersForUser(string userName)
        {
            var user = _userRepository.GetByExpression(u => u.UserName == userName);
            var orders = user.Orders;
            var result = new List<OrderDto>();
            if (orders == null) return result;

            result = orders.Select(so => Mapper.Map<Order, OrderDto>(so)).ToList();
            return result;
        }
        #endregion 
    }
}