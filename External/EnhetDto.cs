using System.Text.Json.Serialization;

namespace CompanyLookup.External
{
    public class EnhetDto
    {
        [JsonPropertyName("organisasjonsnummer")]
        public string? Organisasjonsnummer { get; set; }

        [JsonPropertyName("navn")]
        public string? Navn { get; set; }

        [JsonPropertyName("antallAnsatte")]
        public int? AntallAnsatte { get; set; }

        [JsonPropertyName("organisasjonsform")]
        public OrganisasjonsformDto? Organisasjonsform { get; set; }

        [JsonPropertyName("naeringskode1")]
        public NaeringskodeDto? Naeringskode1 { get; set; }

        // status‑feltene
        [JsonPropertyName("konkurs")]
        public bool Konkurs { get; set; }

        [JsonPropertyName("underAvvikling")]
        public bool UnderAvvikling { get; set; }

        [JsonPropertyName("underTvangsavviklingEllerTvangsopplosning")]
        public bool UnderTvangsavviklingEllerTvangsopplosning { get; set; }

        [JsonPropertyName("slettedato")]
        public string? Slettedato { get; set; }

        public string Status
        {
            get
            {
                if (Konkurs)
                    return "Konkurs";
                if (!string.IsNullOrWhiteSpace(Slettedato))
                    return "Slettet";
                if (UnderAvvikling || UnderTvangsavviklingEllerTvangsopplosning)
                    return "UnderAvvikling";
                return "Aktiv";
            }
        }
    }

    public class OrganisasjonsformDto
    {
        [JsonPropertyName("kode")]
        public string? Kode { get; set; }
    }

    public class NaeringskodeDto
    {
        [JsonPropertyName("kode")]
        public string? Kode { get; set; }
    }

}



