using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceGeneratorService.Utility
{
    internal class TaxCase
    {
        public decimal LandlordRentVat { get; set; } = 0.23M;
        public decimal HousingRentVat { get; set; } = 0.23M;
        public decimal HotWaterVat { get; set; } = 0.08M;
        public decimal ColdWaterVat { get; set; } = 0.08M;
        public decimal GasVat { get; set; } = 0.23M;
        public decimal EnergyVat { get; set; } = 0.23M;
        public decimal HeatVat { get; set; } = 0.23M;
        public decimal TrashVat { get; set; } = 0.08M;
        public decimal GasSubscriptionVat { get; set; } = 0.23M;
        public decimal EnergySubscriptionVat { get; set; } = 0.23M;
        public decimal HeatSubscriptionVat { get; set; } = 0.23M;
        public int Accuracy { get; set; } = 2;

        public MidpointRounding roundingMethod = MidpointRounding.AwayFromZero;

        public readonly Invoice _invoice;
        public readonly Tenant _tenant;
        public readonly Property _property;
        public readonly Landlord _landlord;
        public readonly Rent _rent;


        public TaxCase(Invoice invoice, Tenant tenant, Property property, Landlord landlord, Rent rent) 
        {
            _invoice = invoice;
            _tenant = tenant;
            _property = property;
            _landlord = landlord;
            _rent = rent;
            prepareProperTaxValues();
        }

        public void prepareProperTaxValues()
        {
            //Case 1 (osoba fizyczna wynajmuje osobie fizycznej na cele mieszkaniowe)
            if( !_landlord.IsCompany && !_tenant.IsCompany && _rent.RentPurpose == "live" ) 
            {
                LandlordRentVat = 0;
                HousingRentVat = 0;
                HotWaterVat = 0;
                ColdWaterVat = 0;
                GasVat = 0;
                EnergyVat = 0;
                HeatVat = 0;
                TrashVat = 0;
                GasSubscriptionVat = 0;
                EnergySubscriptionVat = 0;
                HeatSubscriptionVat = 0;
            }
            //Case 2 (osoba fizyczna wynajmuje firmie na cele mieszkaniowe, długoterminowo)
            else if ( !_landlord.IsCompany && _tenant.IsCompany && _rent.RentPurpose == "live" )
            {
                LandlordRentVat = 0;
            }
            //Case 3 (firma wynajmuje firmie na cele mieszkaniowe, długoterminowo)
            else if ( _landlord.IsCompany && _tenant.IsCompany && _rent.RentPurpose == "live" )
            {
                LandlordRentVat = 0;
            }
            //Case 4 (firma wynajmuje firmie na cele prowadzenia działalności gospodarczej, długoterminowo)
            else if (_landlord.IsCompany && _tenant.IsCompany && _rent.RentPurpose == "work")
            {
                
            }
            //Case 5 (osoba fizyczna wynajmuje osobie fizycznej na cele mieszkaniowe, krtótkoterminowo)
            else if (!_landlord.IsCompany && !_tenant.IsCompany && _rent.RentPurpose == "hotel")
            {
                LandlordRentVat = 0.08M;
            }
            //Case 6 (osoba fizyczna wynajmuje osobie fizycznej na cele prowadzenia działalności gospodarczej)
            else if (!_landlord.IsCompany && !_tenant.IsCompany && _rent.RentPurpose == "work")
            {
                
            }
        }

        // jeśli dane pole jest nullable to musimy zastosować metode któa zwróci nam 0 a nie null '.GetValueOrDefault()'

        public decimal GetLandlordRentNet() => decimal.Round(_invoice.LandlordRent, Accuracy, roundingMethod);
        public decimal GetLandlordRentGross() => decimal.Round(GetLandlordRentNet() * LandlordRentVat + GetLandlordRentNet(), Accuracy, roundingMethod);
        public decimal GetLandlordRentTaxValue() => decimal.Round(GetLandlordRentNet() * LandlordRentVat, Accuracy, roundingMethod);

        public decimal GetHousingRentNet() => decimal.Round(_invoice.HousingRent, Accuracy, roundingMethod);
        public decimal GetHousingRentGross() => decimal.Round(GetHousingRentNet() * HousingRentVat + GetHousingRentNet(), Accuracy, roundingMethod);
        public decimal GetHousingRentTaxValue() => decimal.Round(GetHousingRentNet() * HousingRentVat, Accuracy, roundingMethod);

        public decimal GetHotWaterNet() => decimal.Round(_invoice.HotWaterConsumption.GetValueOrDefault() * _invoice.HotWaterPrice.GetValueOrDefault(), Accuracy, roundingMethod);
        public decimal GetHotWaterGross() => decimal.Round(GetHotWaterNet() * HotWaterVat + GetHotWaterNet(), Accuracy, roundingMethod);
        public decimal GetHotWaterTaxValue() => decimal.Round(GetHotWaterNet() * HotWaterVat, Accuracy, MidpointRounding.AwayFromZero);

        public decimal GetColdWaterNet() => decimal.Round(_invoice.ColdWaterConsumption * _invoice.ColdWaterPrice, Accuracy, roundingMethod);
        public decimal GetColdWaterGross() => decimal.Round(GetColdWaterNet() * ColdWaterVat + GetColdWaterNet(), Accuracy, roundingMethod);
        public decimal GetColdWaterTaxValue() => decimal.Round(GetColdWaterNet() * ColdWaterVat, Accuracy, roundingMethod);

        public decimal GetGasNet() => decimal.Round(_invoice.GasConsumption.GetValueOrDefault() * _invoice.GasPrice.GetValueOrDefault(), Accuracy, roundingMethod);
        public decimal GetGasGross() => decimal.Round(GetGasNet() * GasVat + GetGasNet(), Accuracy, roundingMethod);
        public decimal GetGasTaxValue() => decimal.Round(GetGasNet() * GasVat, Accuracy, roundingMethod);

        public decimal GetEnergyNet() => decimal.Round(_invoice.EnergyConsumption * _invoice.EnergyPrice, Accuracy, roundingMethod);
        public decimal GetEnergyGross() => decimal.Round(GetEnergyNet() * EnergyVat + GetEnergyNet(), Accuracy, roundingMethod);
        public decimal GetEnergyTaxValue() => decimal.Round(GetEnergyNet() * EnergyVat, Accuracy, roundingMethod);

        public decimal GetHeatNet() => decimal.Round(_invoice.HeatConsumption.GetValueOrDefault() * _invoice.HeatPrice.GetValueOrDefault(), Accuracy, roundingMethod);
        public decimal GetHeatGross() => decimal.Round(GetHeatNet() * HeatVat + GetHeatNet(), Accuracy, roundingMethod);
        public decimal GetHeatTaxValue() => decimal.Round(GetHeatNet() * HeatVat, Accuracy, roundingMethod);

        public decimal GetGasSubscriptionNet() => decimal.Round(_invoice.GasSubscription, Accuracy, roundingMethod);
        public decimal GetGasSubscriptionGross() => decimal.Round(GetGasSubscriptionNet() * GasSubscriptionVat + GetGasSubscriptionNet(), Accuracy, roundingMethod);
        public decimal GetGasSubscriptionTaxValue() => decimal.Round(GetGasSubscriptionNet() * GasSubscriptionVat, Accuracy, roundingMethod);

        public decimal GetEnergySubscriptionNet() => decimal.Round(_invoice.EnergySubscription, Accuracy, roundingMethod);
        public decimal GetEnergySubscriptionGross() => decimal.Round(GetEnergySubscriptionNet() * EnergySubscriptionVat + GetEnergySubscriptionNet(), Accuracy, roundingMethod);
        public decimal GetEnergySubscriptionTaxValue() => decimal.Round(GetEnergySubscriptionNet() * EnergySubscriptionVat, Accuracy, roundingMethod);

        public decimal GetHeatSubscriptionNet() => decimal.Round(_invoice.HeatSubscription.GetValueOrDefault(), Accuracy, roundingMethod);
        public decimal GetHeatSubscriptionGross() => decimal.Round(GetHeatSubscriptionNet() * HeatSubscriptionVat + GetHeatSubscriptionNet(), Accuracy, roundingMethod);
        public decimal GetHeatSubscriptionTaxValue() => decimal.Round(GetHeatSubscriptionNet() * HeatSubscriptionVat, Accuracy, roundingMethod);

        public decimal GetSummary() =>
            GetLandlordRentGross() +
            GetHousingRentGross() +
            (_property.HasHW ? GetHotWaterGross() : 0) +
            GetColdWaterGross() +
            (_property.HasGas ? GetGasGross() : 0) +
            GetEnergyGross() +
            (_property.HasHeat ? GetHeatGross() : 0) +
            (_property.HasGas ? GetGasSubscriptionGross() : 0) +
            GetEnergySubscriptionGross() +
            (_property.HasHeat ? GetHeatSubscriptionGross() : 0);
    }
}
