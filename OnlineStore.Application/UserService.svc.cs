using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OnlineStore.Infrastructure;
using OnlineStore.ServiceContracts;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application
{
    // UserService.svc, WCF服务
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class UserService : IUserService
    {
        private readonly IUserService _userServiceImp;

        public UserService()
        {
            _userServiceImp = ServiceLocator.Instance.GetService<IUserService>();
        }

        public IList<UserDto> CreateUsers(List<UserDto> userDtos)
        {
            try
            {
                return _userServiceImp.CreateUsers(userDtos);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            try
            {
                return _userServiceImp.ValidateUser(userName, password);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public bool DisableUser(UserDto userDto)
        {
            try
            {
                return _userServiceImp.DisableUser(userDto);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public bool EnableUser(UserDto userDto)
        {
            try
            {
                return _userServiceImp.EnableUser(userDto);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void DeleteUsers(List<UserDto> userDtos)
        {
            try
            {
                _userServiceImp.DeleteUsers(userDtos);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<UserDto> UpdateUsers(List<UserDto> userDataObjects)
        {
            try
            {
                return _userServiceImp.UpdateUsers(userDataObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public UserDto GetUserByKey(Guid id)
        {
            try
            {
                return _userServiceImp.GetUserByKey(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public UserDto GetUserByEmail(string email)
        {
            try
            {
                return _userServiceImp.GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public UserDto GetUserByName(string userName)
        {
            try
            {
                return _userServiceImp.GetUserByName(userName);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<UserDto> GetUsers()
        {
            try
            {
                return _userServiceImp.GetUsers();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<RoleDto> GetRoles()
        {
            try
            {
                return _userServiceImp.GetRoles();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public RoleDto GetRoleByKey(Guid id)
        {
            try
            {
                return _userServiceImp.GetRoleByKey(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<RoleDto> CreateRoles(List<RoleDto> roleDataObjects)
        {
            try
            {
                return _userServiceImp.CreateRoles(roleDataObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<RoleDto> UpdateRoles(List<RoleDto> roleDataObjects)
        {
            try
            {
                return _userServiceImp.UpdateRoles(roleDataObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void DeleteRoles(List<string> roleList)
        {
            try
            {
                _userServiceImp.DeleteRoles(roleList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void AssignRole(Guid userId, Guid roleId)
        {
            try
            {
                _userServiceImp.AssignRole(userId, roleId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public void UnassignRole(Guid userId)
        {
            try
            {
                _userServiceImp.UnassignRole(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public RoleDto GetRoleByUserName(string userName)
        {
            try
            {
                return _userServiceImp.GetRoleByUserName(userName);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }

        public IList<OrderDto> GetOrdersForUser(string userName)
        {
            try
            {
                return _userServiceImp.GetOrdersForUser(userName);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultData>(FaultData.CreateFromException(ex), FaultData.CreateFaultReason(ex));
            }
        }
    }
}
