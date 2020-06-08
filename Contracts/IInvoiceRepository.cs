using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IInvoiceRepository: IRepositoryBase<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(int id);
        Task<IEnumerable<Invoice>> GetInvoicesByRentId(int id);
        Task<IEnumerable<Invoice>> GetInvoicesWithStateByRentId(int id);
        Task<Invoice> GetInvoiceWithRentByInvoiceId(int id);
        Task<Invoice> GetInvoiceWithStateRentAndPropertyAndAddressByInvoiceId(int id);
        Task<Invoice> GetInvoiceByStateId(int id);
        Task<Invoice> GetInvoiceWithStateByInvoiceId(int id);
        Task<Invoice> GetInvoiceWihtAllDetailsForInvoiceGeneration(int id);
        Task<Boolean> CheckIfInvoiceExistByStateId(int id);
        void CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(Invoice invoice);
    }

   
}
