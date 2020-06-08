using Contracts;
using Entities;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Wrapper sets out repositories
    /// </summary>
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private ILandlordRepository _landlord;
        private IAddressRepository _address;
        private ITenantRepository _tenant;
        private IPropertyRepository _property;
        private IInvoiceRepository _invoice;
        private IPhotoRepository _photo;
        private IRateRepository _rate;
        private IRentRepository _rent;
        private IStateRepository _state;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ILandlordRepository Landlord
        {
            get
            {
                if(_landlord == null)
                    _landlord = new LandlordRepository(_repositoryContext);
                return _landlord;
            }
        }

        public IAddressRepository Address
        {
            get
            {
                if (_address == null)
                    _address = new AddressRepository(_repositoryContext);
                return _address;
            }
        }

        public ITenantRepository Tenant
        {
            get
            {
                if (_tenant == null)
                    _tenant = new TenantRepository(_repositoryContext);
                return _tenant;
            }
        }

        public IPropertyRepository Property
        {
            get
            {
                if (_property == null)
                    _property = new PropertyRepository(_repositoryContext);
                return _property;
            }
        }

        public IInvoiceRepository Invoice
        {
            get
            {
                if(_invoice== null)
                    _invoice = new InvoiceRepository(_repositoryContext);
                return _invoice;
            }
        }

        public IPhotoRepository Photo
        {
            get
            {
                if (_photo == null)
                    _photo = new PhotoRepository(_repositoryContext);
                return _photo;
            }
        }

        public IRateRepository Rate
        {
            get
            {
                if (_rate == null)
                    _rate = new RateRepository(_repositoryContext);
                return _rate;
            }
        }

        public IRentRepository Rent
        {
            get
            {
                if (_rent == null)
                    _rent = new RentRepository(_repositoryContext);
                return _rent;
            }
        }

        public IStateRepository State
        {
            get
            {
                if (_state == null)
                    _state = new StateRepository(_repositoryContext);
                return _state;
            }
        }

        public async Task Save()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
