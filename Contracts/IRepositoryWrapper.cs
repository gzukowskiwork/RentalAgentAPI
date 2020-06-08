using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        ILandlordRepository Landlord {get;}
        IInvoiceRepository Invoice { get; }
        IAddressRepository Address { get; }
        IPhotoRepository Photo { get; }
        IPropertyRepository Property { get; }
        ITenantRepository Tenant { get; }
        IRateRepository Rate { get; }
        IRentRepository Rent { get; }
        IStateRepository State { get; }
        Task Save();
    }
}
