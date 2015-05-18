using AutoMapper;
using OnlineStore.Domain.Model;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application
{
    public class AddressResolver : ValueResolver<AddressDto, Address>
    {
        protected override Address ResolveCore(AddressDto source)
        {
            return new Address
            {
                City = source.City,
                Country = source.Country,
                State = source.State,
                Street = source.Street,
                Zip = source.Zip
            };
        }
    }
}