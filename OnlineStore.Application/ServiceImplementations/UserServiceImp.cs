using System;
using System.Collections.Generic;
using AutoMapper;
using OnlineStore.Domain.Model;
using OnlineStore.Domain.Repositories;
using OnlineStore.ServiceContracts;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application.ServiceImplementations
{
    public class UserServiceImp :ApplicationService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public UserServiceImp(IRepositoryContext repositoryContext, 
            IUserRepository userRepository, 
            IShoppingCartRepository shoppingCartRepository)
            : base(repositoryContext)
        {
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IList<UserDto> CreateUsers(List<UserDto> userDtos)
        {
            if (userDtos == null)
                throw new ArgumentNullException("userDtos");
            return PerformCreateObjects<List<UserDto>, UserDto, User>(userDtos,
                _userRepository,
                dto =>
                {
                    if (dto.RegisterDate == null)
                        dto.RegisterDate = DateTime.Now;
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
            throw new NotImplementedException();
        }

        public void DeleteUsers(UserDto userDto)
        {
            throw new System.NotImplementedException();
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
    }
}