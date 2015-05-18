using AutoMapper;
using OnlineStore.Domain.Model;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.Application
{
    public class InversedAddressResolver : ValueResolver<Address, AddressDto>
    {
        protected override AddressDto ResolveCore(Address source)
        {
            return new AddressDto
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