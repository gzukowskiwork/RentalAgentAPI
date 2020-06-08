using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace InvoiceGeneratorService.Utility
{
    public static class SystemRecognizer
    {
        public static readonly string osNameAndVersion = RuntimeInformation.OSDescription;
        public static readonly bool runOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool runOnLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static readonly bool runOnOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        private static readonly string windowsCssDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../InvoiceGeneratorService/assets", "styles.css");
        private static readonly string linuxCssDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../../../../InvoiceGeneratorService/assets", "styles.css");
        private static readonly string windowsInvoiceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../InvoiceGeneratorService/Invoices", "Invoice.pdf");
        private static readonly string linuxInvoiceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../../../../InvoiceGeneratorService/Invoices", "Invoice.pdf");

        public static string GetCssFileLocation()
        {
            string cssLocation = runOnWindows ? windowsCssDirectory : 
                                 runOnLinux ? linuxCssDirectory : 
                                 "unknownSystem";
            return cssLocation;
        }

        public static string GetGeneratedInvoiceFileLocation()
        {
            string cssLocation = runOnWindows ? windowsInvoiceDirectory :
                                 runOnLinux ? linuxInvoiceDirectory :
                                 "unknownSystem";
            return cssLocation;
        }


    }
}
