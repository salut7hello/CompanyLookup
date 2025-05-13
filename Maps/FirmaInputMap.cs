using CsvHelper.Configuration;           // Mappen for ClassMap
using CompanyLookup.Models;             // Der FirmaInput ligger

namespace CompanyLookup.Maps
{
    public sealed class FirmaInputMap : ClassMap<FirmaInput>
    {
        public FirmaInputMap()
        {
            // “OrgNr”-kolonnen går til FirmaNavn
            Map(m => m.FirmaNavn).Name("OrgNr");
            // “FirmaNavn”-kolonnen går til OrgNo
            Map(m => m.OrgNr).Name("FirmaNavn");
        }
    }
}