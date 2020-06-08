using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Invoice
{
    public class InvoiceDocumentDto
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public int StateId { get; set; }
        public Byte[] InvoiceDocument { get; set; }
        public string FileName { get; set; }
        public bool IsDistributed { get; set; }

    }
}
