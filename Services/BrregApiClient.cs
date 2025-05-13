
using System.Text.Json;
using CompanyLookup.External;

namespace CompanyLookup.Services;

public class BrregApiClient
{
    private readonly HttpClient _http;

    public BrregApiClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://data.brreg.no/enhetsregisteret/api/");
        _http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        _http.Timeout = TimeSpan.FromSeconds(10);
    }


    public async Task<EnhetDto?> GetEnhetAsync(string orgNr, string firmaNavn)
    {
        if (string.IsNullOrWhiteSpace(orgNr))
            throw new ArgumentException("OrgNr må være et 9-sifret tall.", nameof(orgNr));
        var endpoint = $"enheter/{orgNr.Trim()}";
        try
        {
            var response = await _http.GetAsync(endpoint);
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    // Fortsett til deserialisering
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    Console.WriteLine($"OrgNr: {orgNr}, FirmaNavn: {firmaNavn} - Feilaktig forespørsel (400)");
                    return null;
                case System.Net.HttpStatusCode.NotFound:
                    Console.WriteLine($"OrgNr: {orgNr}, FirmaNavn: {firmaNavn} - Ikke funnet (404)");
                    return null;
                case System.Net.HttpStatusCode.Gone:  // 410
                    Console.WriteLine($"OrgNr: {orgNr}, FirmaNavn: {firmaNavn} - Fjernet av juridiske årsaker (410)");
                    return null;
                case System.Net.HttpStatusCode.InternalServerError:
                    Console.WriteLine($"OrgNr: {orgNr}, FirmaNavn: {firmaNavn} - Serverfeil (500)");
                    return null;
                default:
                    Console.WriteLine($"OrgNr: {orgNr}, FirmaNavn: {firmaNavn} - HTTP {(int)response.StatusCode}");
                    return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonSerializer.Deserialize<EnhetDto>(json);
            }
            catch (JsonException e)
            {
                Console.WriteLine($"JSON parse error for OrgNr: {orgNr}: {e.Message}");
                return null;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"HTTP request error for OrgNr: {orgNr}: {e.Message}");
            return null;
        }
    }
}

