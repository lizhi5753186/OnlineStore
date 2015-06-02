using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.Web.Security;
using OnlineStore.Web.UserService;
// ReSharper disable All

namespace OnlineStore.Web
{
    public class OnlineStoreMembershipProvider : MembershipProvider
    {
        private string applicationName;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval = false;
        private bool requireQuestionAndAnswer = false;
        private bool requireUniqueEmail = true;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private int minRequiredPasswordLength;
        private int minRequiredNonalphanumericCharacters;
        private string passwordStrengthRegularExpression;
        private MembershipPasswordFormat passwordFormat = MembershipPasswordFormat.Clear;

        private OnlineStoreMembershipUser ConvertFrom(UserDto userDto)
        {
            if (userDto == null)
                return null;

            var user = new OnlineStoreMembershipUser("OnlineStoreMembershipProvider",
                userDto.UserName,
                userDto.Id,
                userDto.Email,
                "",
                "",
                true,
                userDto.IsDisabled ?? true,
                userDto.RegisteredDate ?? DateTime.MinValue,
                userDto.LastLogonDate ?? DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                userDto.Contact,
                userDto.PhoneNumber,
                userDto.ContactAddress,
                userDto.DeliveryAddress);

            return user;
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public OnlineStoreMembershipUser CreateUser(string username,
           string password,
           string email,
           string passwordQuestion,
           string passwordAnswer,
           bool isApproved,
           object providerUserKey,
           string contact,
           string phoneNumber,
           AddressDto contactAddress,
           AddressDto deliveryAddress,
           out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            var user = GetUser(username, true) as OnlineStoreMembershipUser;
            if (user == null)
            {
                using (var proxy = new UserServiceClient())
                {
                    List<UserDto> userDtos = new List<UserDto>
                    {
                        new UserDto
                        {
                            UserName = username,
                            Password = password,
                            Contact = contact,
                            LastLogonDate = null,
                            RegisteredDate = DateTime.Now,
                            Email = email,
                            IsDisabled = false,
                            PhoneNumber = phoneNumber,
                            ContactAddress = contactAddress,
                            DeliveryAddress = deliveryAddress
                        }
                    };

                    proxy.CreateUsers(userDtos.ToArray());
                }

                status = MembershipCreateStatus.Success;
                return GetUser(username, true) as OnlineStoreMembershipUser;
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }
        }

        #region MembershipProvider Members

        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name))
                name = "OnlineStoreMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Membership Provider for OnlineStore");
            }

            base.Initialize(name, config);

            ApplicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredNonalphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, null, null, null, null, out status);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new System.NotImplementedException();
        }

        public override bool EnablePasswordReset 
        {
            get 
            {
                return enablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (var proxy = new UserServiceClient())
            {
                var userDto = proxy.GetUserByName(username);
                if (userDto == null)
                    return null;
                return ConvertFrom(userDto);
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            using (var proxy = new UserServiceClient())
            {
                var userDto = proxy.GetUserByKey((Guid) providerUserKey);
                if (userDto == null)
                    return null;
                return ConvertFrom(userDto);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (var proxy = new UserServiceClient())
            {
                var userDto = proxy.GetUserByEmail(email);
                if(userDto == null)
                    return null;
                return userDto.UserName;
            }
        }

        public override int MaxInvalidPasswordAttempts {
            get { return maxInvalidPasswordAttempts;}
        }

        public override int MinRequiredNonAlphanumericCharacters 
        {
            get { return minRequiredNonalphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength 
        {
            get { return minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow 
        {
            get { return passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return this.passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return this.passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return this.requireQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return requireUniqueEmail; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (var proxy = new UserServiceClient())
            {
                return proxy.ValidateUser(username, password);
            }
        }

        #endregion 
    }
}