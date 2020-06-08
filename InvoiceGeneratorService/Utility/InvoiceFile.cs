using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceGeneratorService.Utility
{
    public class InvoiceFile
    {
        private string _ivoiceString { get; set; }
        private string _invoiceNumber { get; set; }
        private byte[] _invoiceBase64 { get; set; }

        public InvoiceFile(string invoiceName, string invoiceString)
        {
            this._ivoiceString = invoiceString;
            this._invoiceNumber = invoiceName;
        }

        public string getInvoiceString()
        {
            return this._ivoiceString;
        }

        public string getInvoiceNumber()
        {
            return this._invoiceNumber;
        }

        public string getInvoiceFileName()
        {
            return this._invoiceNumber + ".pdf";
        }

        public byte[] getInvoiceBytes()
        {
            return this._invoiceBase64;
        }

        public void setInvoiceBytes(byte[] invoiceB64)
        {
            this._invoiceBase64 = invoiceB64;
        }

    }
}
