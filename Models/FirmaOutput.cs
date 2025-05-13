namespace CompanyLookup.Models
{
    public record FirmaOutput(
        string OrgNo,
        string FirmaNavn,
        string Status,
        int? AntallAnsatte,
        string? OrganisasjonsformKode,
        string? Naeringskode
    );
}
