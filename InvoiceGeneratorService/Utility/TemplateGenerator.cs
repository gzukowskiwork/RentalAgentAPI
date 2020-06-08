using Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InvoiceGeneratorService.Utility
{

    public static class TemplateGenerator
    {
        private static readonly DateTime date = DateTime.Now;

        public static InvoiceFile GetInvoiceHTMLString(ApplicationUser landlordAspUser, ApplicationUser tenantAspUser, Invoice invoice, Tenant tenant, Property property, Landlord landlord, Rent rent)
        {
            uint index = 0;
            TaxCase tax = new TaxCase(invoice, tenant, property, landlord, rent);

            string logoToString = Convert.ToBase64String(landlord.Logo);
            string filePath = "./CompanyLogo.png";
            string invoiceNumber = $"{invoice.Id}/{date.Date.Month}/{date.Date.Year}";

            File.WriteAllBytes(filePath, Convert.FromBase64String(logoToString));
            String logoPath = Path.Combine(Directory.GetCurrentDirectory(), "./", "CompanyLogo.png");

            var sb = new StringBuilder();

            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>");

            if (landlord.Logo != null)
            {
                sb.AppendFormat(@$" 
                                <div class='logo'>
                                      <img src='{logoPath}' alt ='Company Logo' />
                                </div>");
            }

            sb.AppendFormat(@$"
                                <div class='dateWrapper'>
                                    <div class='date'>Miejsce i data wystawienia: {landlord.Address.City}, {date.Date.ToString("dd/MM/yyyy")}</div>
                                    <div class='date sellDate'>Data sprzedaży: {date.Date.ToString("dd/MM/yyyy")}
                                    <div class='date paymentDate'>Termin zapłaty: {date.Date.AddDays(rent.PayDayDelay).ToString("dd/MM/yyyy")}</div>
                                </div>

                                <div class='header'>
                                    <h1>Faktura nr: {invoice.Id}/{date.Date.Month}/{date.Date.Year}</h1>
                                    <h2> za okres: {date.AddMonths(-1).Month.ToString("d")}/{date.Date.Year.ToString("d")} r.</h2>
                                </div>
                                

                                <div class='addressWrapper'>
                                    <table class='landlordAddress'>
                                        <tr><th>Wynajmujący:</th></tr>
                                        <tr><th>{landlord.Name} {landlord.Surname}</th></tr>
                                        <tr><th>ul.{landlord.Address.Street} {landlord.Address.BuildingNumber}/{landlord.Address.FlatNumber}</th></tr>
                                        <tr><th>{landlord.Address.PostalCode} {landlord.Address.City}</th></tr>
                                        <tr><th>{(landlord.IsCompany ? $"NIP:{landlord.NIP}" : $"PESEL:{landlord.PESEL}")}</th></tr>
                                        <tr><th>tel.{landlord.PhonePrefix} {landlord.PhoneNumber}</th></tr>
                                        <tr><th>{landlordAspUser.Email}</th></tr>
                                    </table>

                                    <table class='tenantAddress'>
                                        <tr><th>Najemca:</th></tr>
                                        <tr><th>{tenant.Name} {tenant.Surname}</th></tr>
                                        <tr><th>ul.{tenant.Address.Street} {tenant.Address.BuildingNumber}/{tenant.Address.FlatNumber}</th></tr>
                                        <tr><th>{tenant.Address.PostalCode} {tenant.Address.City}</th></tr>
                                        <tr><th>{ (tenant.IsCompany ? $"NIP:{tenant.NIP}" : $"PESEL:{tenant.PESEL}") }</th></tr> 
                                        <tr><th>tel. {tenant.PhonePrefix} {tenant.PhoneNumber}</th></tr>
                                        <tr><th>{tenantAspUser.Email}</th></tr>
                                        <tr><th>{rent.RentPurpose}</th></tr>
                                    </table>

                                    <table class='propertyAddress'>
                                        <tr><th>Mieszkanie:</th></tr>
                                        <tr><th>{property.FlatLabel}</th></tr>
                                        <tr><th>ul. {property.Address.Street} {property.Address.BuildingNumber}/{property.Address.FlatNumber}</th></tr>
                                        <tr><th>{property.Address.PostalCode} {property.Address.City}</th></tr>
                                        <tr><th>{property.FlatSize}[m<sup>2</sup>]</th></tr>   
                                    </table>
                                </div>
                            ");



                    sb.Append(@"
                                <div class='invoicePositionsWrapper'>
                                    <table class='positions'>
                                        <tr>
                                            <th>LP</th>
                                            <th>NAZWA</th>
                                            <th>J.M.</th>
                                            <th>Ilość</th>
                                            <th>Cena [PLN]</th>
                                            <th>Wartość Netto [PLN]</th>
                                            <th>Vat %</th>
                                            <th>Kwota Podatku</th>
                                            <th>Wartość brutto [PLN]</th>
                                        </tr>");
                   
                    sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Czynsz najmu mieszkania zgodnie z umową</td>
                                            <td>usł.</td>
                                            <td>1,00</td>
                                            <td>{tax.GetLandlordRentNet()}</td>
                                            <td>{tax.GetLandlordRentNet()}</td>
                                            <td>{(tax.LandlordRentVat == 0M ? "zw." : $"{tax.LandlordRentVat * 100}" )}</td>
                                            <td>{tax.GetLandlordRentTaxValue()}</td>
                                            <td>{tax.GetLandlordRentGross()}</td>
                                        </tr>");

                    sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Czynsz do wspólnoty</td>
                                            <td>usł.</td>
                                            <td>1,00</td>
                                            <td>{tax.GetHousingRentNet()}</td> 
                                            <td>{tax.GetHousingRentNet()}</td>
                                            <td>{(tax.HousingRentVat == 0 ? "np." : $"{tax.HousingRentVat * 100}")}</td>
                                            <td>{tax.GetHousingRentTaxValue()}</td>
                                            <td>{tax.GetHousingRentGross()}</td>
                                        </tr>"); 

                    sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Zużycie woda zimna</td>
                                            <td>[m<sup>3</sup>]</td>
                                            <td>{invoice.ColdWaterConsumption}</td>
                                            <td>{invoice.ColdWaterPrice}</td> 
                                            <td>{tax.GetColdWaterNet()}</td>
                                            <td>{(tax.ColdWaterVat == 0 ? "np." : $"{tax.ColdWaterVat * 100}")}</td>
                                            <td>{tax.GetColdWaterTaxValue()}</td>
                                            <td>{tax.GetColdWaterGross()}</td>
                                        </tr>");
            if (property.HasHW)
            {
                sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Zużycie woda ciepła</td>
                                            <td>[m<sup>3</sup>]</td>
                                            <td>{invoice.HotWaterConsumption}</td>
                                            <td>{invoice.HotWaterPrice}</td> 
                                            <td>{tax.GetHotWaterNet()}</td>
                                            <td>{(tax.HotWaterVat == 0 ? "np." : $"{tax.HotWaterVat * 100}")}</td>
                                            <td>{tax.GetHotWaterTaxValue()}</td>
                                            <td>{tax.GetHotWaterGross()}</td>
                                        </tr>");
            }

            if(property.HasGas)
            {
                    sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Zużycie gaz</td>
                                            <td>[m<sup>3</sup>]</td>
                                            <td>{invoice.GasConsumption}</td>
                                            <td>{invoice.GasPrice}</td>
                                            <td>{tax.GetGasNet()}</td>
                                            <td>{(tax.GasVat == 0 ? "np." : $"{tax.GasVat * 100}")}</td>
                                            <td>{tax.GetGasTaxValue()}</td>
                                            <td>{tax.GetGasGross()}</td>
                                        </tr>");
            }

                    sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Zużycie energia</td>
                                            <td>[kWh]</td>
                                            <td>{invoice.EnergyConsumption}</td>
                                            <td>{invoice.EnergyPrice}</td>
                                            <td>{tax.GetEnergyNet()}</td>
                                            <td>{(tax.EnergyVat == 0 ? "np." : $"{tax.EnergyVat * 100}")}</td>
                                            <td>{tax.GetEnergyTaxValue()}</td>
                                            <td>{tax.GetEnergyGross()}</td>
                                        </tr>");
            if (property.HasHeat)
            {
                    sb.AppendFormat(@$"<tr>
                                             <td>{++index}</td>
                                             <td>Zużycie ciepła</td>
                                             <td>[gJ]</td>
                                             <td>{invoice.HeatConsumption}</td>
                                             <td>{invoice.HeatPrice}</td>
                                             <td>{tax.GetHeatNet()}</td>
                                             <td>{(tax.HeatVat == 0 ? "np." : $"{tax.HeatVat * 100}")}</td>
                                             <td>{tax.GetHeatTaxValue()}</td>
                                             <td>{tax.GetHeatGross()}</td>
                                         </tr>");
            }

            if (property.HasGas)
            {
                sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Abonament gaz</td>
                                            <td>usł.</td>
                                            <td>1,00</td>
                                            <td>{invoice.GasSubscription}</td> 
                                            <td>{invoice.GasSubscription}</td>
                                            <td>{(tax.GasVat == 0 ? "np." : $"{tax.GasSubscriptionVat * 100}")}</td>
                                            <td>{tax.GetGasSubscriptionTaxValue()}</td>
                                            <td>{tax.GetGasSubscriptionGross()}</td>
                                        </tr>");
            }
                    sb.AppendFormat(@$"<tr>
                                            <td>{++index}</td>
                                            <td>Abonament energia</td>
                                            <td>usł.</td>
                                            <td>1,00</td>
                                            <td>{invoice.EnergySubscription}</td> 
                                            <td>{invoice.EnergySubscription}</td>
                                            <td>{(tax.EnergyVat == 0 ? "np." : $"{tax.EnergySubscriptionVat * 100}")}</td>
                                            <td>{tax.GetEnergySubscriptionTaxValue()}</td>
                                            <td>{tax.GetEnergySubscriptionGross()}</td>
                                        </tr>");
            if (property.HasHeat)
            {
                sb.AppendFormat(@$"<tr>
                                             <td>{++index}</td>
                                             <td>Abonament ciepło</td>
                                             <td>usł.</td>
                                             <td>1,00</td>
                                             <td>{invoice.HeatSubscription}</td>
                                             <td>{invoice.HeatSubscription}</td>
                                             <td>{(tax.HeatVat == 0 ? "np." : $"{tax.HeatSubscriptionVat * 100}")}</td>
                                             <td>{tax.GetHeatSubscriptionTaxValue()}</td>
                                             <td>{tax.GetHeatSubscriptionGross()}</td>
                                         </tr>");
            }

                     sb.AppendFormat(@$"<tr>
                                            <td></td>
                                            <td>Razem do zapłaty</td>
                                            <td></td>
                                            <td></td>
                                            <td></td> 
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td>{tax.GetSummary()}</td>
                                        </tr>");



                     sb.Append(@$"
                                    </table>
                                </div> <!-- END .invoicePositionsWrapper-->");
            if (invoice.LandlordComment != null)
            {
                sb.Append(@$"                 
                               <div class='comment'>Komentarz Wynajmujacego: {invoice.LandlordComment}</div>");
            }
                sb.Append(@$"
                               <div class='bankAccount'>Rachunek bankowy do wpłaty należności: {landlord.BankAccount}</div>

                            </body>
                        </html>");

            return new InvoiceFile(invoiceNumber, sb.ToString());
        }


    }
}
