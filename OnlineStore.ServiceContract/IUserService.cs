using System;
using System.Collections.Generic;
using System.ServiceModel;
using OnlineStore.Infrastructure.Caching;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.ServiceContracts
{
    // 用户服务契约
    [ServiceContract(Namespace = "")]
    public interface IUserService
    {
        #region Methods

        [OperationContract]
        [FaultContract(typeof (FaultData))]
        IList<UserDto> CreateUsers(List<UserDto> userDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool ValidateUser(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DisableUser(UserDto userDto);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool EnableUser(UserDto userDto);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteUsers(List<UserDto> userDtos);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<UserDto> UpdateUsers(List<UserDto> userDataObjects);

        [OperationContract]
        [FaultContract(typeof (FaultData))]
        UserDto GetUserByKey(Guid id);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDto GetUserByEmail(string email);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDto GetUserByName(string userName);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<UserDto> GetUsers();

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<RoleDto> GetRoles();

        [FaultContract(typeof(FaultData))]
        RoleDto GetRoleByKey(Guid id);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<RoleDto> CreateRoles(List<RoleDto> roleDataObjects);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<RoleDto> UpdateRoles(List<RoleDto> roleDataObjects);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteRoles(List<string> roleList);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetRoleByUserName")]
        void AssignRole(Guid userId, Guid roleId);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Remove, "GetRoleByUserName")]
        void UnassignRole(Guid userId);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Cache(CachingMethod.Get)]
        RoleDto GetRoleByUserName(string userName);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        IList<OrderDto> GetOrdersForUser(string userName);
        #endregion
    }
}