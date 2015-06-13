using System;

namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? IsDisabled { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public DateTime? LastLogonDate { get; set; }
        public string Contact { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto ContactAddress { get; set; }
        public AddressDto DeliveryAddress { get; set; }

        public RoleDto Role { get; set; }

        public override string ToString()
        {
            return this.UserName;
        }
    }
}