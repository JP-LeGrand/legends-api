using AutoMapper;
using legend.Entities;
using legend.Helpers;
using legend.Models.Address;

namespace legend.Services
{
    public interface IAddressService
    {
        IEnumerable<Address> GetAll();
        IEnumerable<Address> GetUserAddress(Guid userId);
        Address GetById(Guid id);
        void Add(AddAddressRequest model);
        void Update(Guid id, UpdateAddressRequest model);
        void Delete(Guid id);
    }

    public class AddressService : IAddressService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AddressService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        IEnumerable<Address> IAddressService.GetAll()
        {
            return _context.Addresses;
        }

        Address IAddressService.GetById(Guid id)
        {
            return GetAddress(id);
        }

        void IAddressService.Add(AddAddressRequest model)
        {
            // map model to new address object
            var address = _mapper.Map<Address>(model);

            // save address
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }

        void IAddressService.Update(Guid id, UpdateAddressRequest model)
        {
            var address = GetAddress(id);

            // copy model to user and save
            _mapper.Map(model, address);
            _context.Addresses.Update(address);
            _context.SaveChanges();
        }

        void IAddressService.Delete(Guid id)
        {
            var address = GetAddress(id);
            _context.Addresses.Remove(address);
            _context.SaveChanges();
        }

        // helper methods

        private Address GetAddress(Guid id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null) throw new KeyNotFoundException("Address not found");
            return address;
        }

        IEnumerable<Address> IAddressService.GetUserAddress(Guid userId)
        {
            return _context.Addresses.Where(a => a.UserId == userId);
        }
    }
}

