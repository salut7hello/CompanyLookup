using CompanyLookup.Models;
using CompanyLookup.External;

namespace CompanyLookup.Services
{

    public static class FirmaOutputMapper
    {
        public static FirmaOutput Map(EnhetDto dto)
        {

            return new FirmaOutput(
                OrgNo: dto.Organisasjonsnummer,
                FirmaNavn: dto.Navn,
                Status: dto.Status ?? "Feil",
                AntallAnsatte: dto.AntallAnsatte,
                OrganisasjonsformKode: dto.Organisasjonsform?.Kode,
                Naeringskode: dto.Naeringskode1?.Kode
               );

        }
    }
}
