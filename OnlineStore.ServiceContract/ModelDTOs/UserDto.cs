using System;

namespace OnlineStore.ServiceContracts.ModelDTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? IsDisable { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Contact { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto ContactAddress { get; set; }
        public AddressDto DeliveryAddress { get; set; }
    }
}