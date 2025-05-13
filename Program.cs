using System;
using System.Text.Json;
using CompanyLookup.Models;
using CompanyLookup.External;
using CompanyLookup.Services;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "stats")
        {
            await StatsMode();
            return;
        }

        await RunImportAsync();
    }

    static async Task RunImportAsync()
    {
        // lese csv fil
        var firmaer = await CsvService.LoadFirmaInputAsync("firmaer.csv");
        //  Initialiser Brreg-klienten
        var brregClient = new BrregApiClient(new HttpClient());
        // Hent og map utdata
        var outputs = new List<FirmaOutput>();

        foreach (var f in firmaer)
        {
            // Hent DTO fra API
            var dto = await brregClient.GetEnhetAsync(f.OrgNr, f.FirmaNavn);
            if (dto == null)
            {
                Console.WriteLine($"Feil eller ingen data for {f.OrgNr}");
                continue;
            }

            var output = FirmaOutputMapper.Map(dto);

            outputs.Add(output);
        }
        // Skriv alle outputs til CSV
        await CsvService.WriteFirmaOutputAsync("firmaer_output.csv", outputs);

    }

    static async Task StatsMode()
    {
        var statsSvc = new StatsService();
        var rows = await statsSvc.LoadAsync("firmaer_output.csv");

        statsSvc.CalculateAndPrint(rows);
            
    }
}
