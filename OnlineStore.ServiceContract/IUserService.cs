using System;
using System.Collections.Generic;
using System.ServiceModel;
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
        void DeleteUsers(UserDto userDto);

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

        #endregion
    }
}