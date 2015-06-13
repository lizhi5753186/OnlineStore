using System;
using System.ComponentModel.DataAnnotations;
using OnlineStore.Web.UserService;

// ReSharper disable All

namespace OnlineStore.Web.ViewModels
{
    public class UserAccountModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请重新输入密码以便确认")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码与输入的密码不符")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "请输入电子邮件")]
        [DataType(DataType.EmailAddress, ErrorMessage = "电子邮件格式不正确")]
        public string Email { get; set; }

        [Display(Name = "已禁用")]
        public bool? IsDisabled { get; set; }

        [Display(Name = "注册时间")]
        [DataType(DataType.Date)]
        public DateTime? RegisteredDate { get; set; }

        [Display(Name = "注册时间")]
        public string RegisteredDateStr
        {
            get { return RegisteredDate.HasValue ? RegisteredDate.Value.ToShortDateString() : "N/A"; }
        }

        [Display(Name = "角色")]
        public RoleDto Role { get; set; }

        [Display(Name = "角色")]
        public string RoleStr
        {
            get
            {
                if (Role != null && !string.IsNullOrEmpty(Role.Name))
                    return Role.Name;
                return "(未指定)";
            }
        }

        [Display(Name = "最后登录")]
        [DataType(DataType.Date)]
        public DateTime? LastLogonDate { get; set; }

        [Display(Name = "最后登录")]
        public string LastLogonDateStr
        {
            get { return LastLogonDate.HasValue ? LastLogonDate.Value.ToShortDateString() : "N/A"; }
        }

        [Display(Name = "联系人")]
        [Required(ErrorMessage = "请输入联系人")]
        public string Contact { get; set; }

        [Display(Name = "电话号码")]
        [Required(ErrorMessage = "请输入电话号码")]
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)",
            ErrorMessage = "电话号码格式不正确")]
        public string PhoneNumber { get; set; }

        [Display(Name = "联系地址 - 国家")]
        [Required(ErrorMessage = "请输入国家")]
        public string ContactAddressCountry { get; set; }

        [Display(Name = "联系地址 - 省/州")]
        [Required(ErrorMessage = "请输入省/州")]
        public string ContactAddressState { get; set; }

        [Display(Name = "联系地址 - 市")]
        [Required(ErrorMessage = "请输入市")]
        public string ContactAddressCity { get; set; }

        [Display(Name = "联系地址 - 街道")]
        [Required(ErrorMessage = "请输入街道")]
        public string ContactAddressStreet { get; set; }

        [Display(Name = "联系地址 - 邮编")]
        [Required(ErrorMessage = "请输入邮编")]
        public string ContactAddressZip { get; set; }

        [Display(Name = "收货地址 - 国家")]
        [Required(ErrorMessage = "请输入国家")]
        public string DeliveryAddressCountry { get; set; }

        [Display(Name = "收货地址 - 省/州")]
        [Required(ErrorMessage = "请输入省/州")]
        public string DeliveryAddressState { get; set; }

        [Display(Name = "收货地址 - 市")]
        [Required(ErrorMessage = "请输入市")]
        public string DeliveryAddressCity { get; set; }

        [Display(Name = "收货地址 - 街道")]
        [Required(ErrorMessage = "请输入街道")]
        public string DeliveryAddressStreet { get; set; }

        [Display(Name = "收货地址 - 邮编")]
        [Required(ErrorMessage = "请输入邮编")]
        public string DeliveryAddressZip { get; set; }

        public override string ToString()
        {
            return UserName;
        }

        public static UserAccountModel CreateFromDto(UserDto d)
        {
            return new UserAccountModel
            {
                Id = d.Id,
                UserName = d.UserName,
                Password = d.Password,
                Email = d.Email,
                IsDisabled = d.IsDisabled,
                RegisteredDate = d.RegisteredDate,
                LastLogonDate = d.LastLogonDate,
                Role = d.Role,
                Contact = d.Contact,
                PhoneNumber = d.PhoneNumber,
                ContactAddressCity = d.ContactAddress.City,
                ContactAddressStreet = d.ContactAddress.Street,
                ContactAddressState = d.ContactAddress.State,
                ContactAddressCountry = d.ContactAddress.Country,
                ContactAddressZip = d.ContactAddress.Zip,
                DeliveryAddressCity = d.DeliveryAddress.City,
                DeliveryAddressStreet = d.DeliveryAddress.Street,
                DeliveryAddressState = d.DeliveryAddress.State,
                DeliveryAddressCountry = d.DeliveryAddress.Country,
                DeliveryAddressZip = d.DeliveryAddress.Zip,
            };
        }

        public UserDto ConvertToDto()
        {
            return new UserDto()
            {
                Id = this.Id,
                UserName = this.UserName,
                Password = this.Password,
                IsDisabled = this.IsDisabled,
                Email = this.Email,
                RegisteredDate = this.RegisteredDate,
                LastLogonDate = this.LastLogonDate,
                Contact = this.Contact,
                PhoneNumber = this.PhoneNumber,
                ContactAddress = new AddressDto()
                {
                    Country = ContactAddressCountry,
                    State = ContactAddressState,
                    City = ContactAddressCity,
                    Street = ContactAddressStreet,
                    Zip = ContactAddressZip
                },

                DeliveryAddress = new AddressDto()
                {
                    Country = DeliveryAddressCountry,
                    State = DeliveryAddressState,
                    City = DeliveryAddressCity,
                    Street = DeliveryAddressStreet,
                    Zip = DeliveryAddressZip
                }
            };

        }
    }
}