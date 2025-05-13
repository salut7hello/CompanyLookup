using System.Globalization;
using CompanyLookup.Models;
using CsvHelper;
using CsvHelper.Configuration;

using CompanyLookup.Maps;
namespace CompanyLookup.Services

{
    public static class CsvService
    {
        //lese CSV fil
        public static async Task<List<FirmaInput>> LoadFirmaInputAsync(string inputPath)
        {
            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(inputPath);
            using var csv = new CsvReader(reader, cfg);

            csv.Context.RegisterClassMap<FirmaInputMap>();

            var list = new List<FirmaInput>();
            await foreach (var rec in csv.GetRecordsAsync<FirmaInput>())
            {
                list.Add(rec);
            }
            return list;
        }
        // Skriv alle outputs til CSV
        public static async Task WriteFirmaOutputAsync(string outputPath, List<FirmaOutput> outputs)
        {

            using var writer = new StreamWriter(outputPath);
            using var csvOut = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            });
            csvOut.WriteHeader<FirmaOutput>();
            await csvOut.NextRecordAsync();
            foreach (var rec in outputs)
            {
                csvOut.WriteRecord(rec);
                await csvOut.NextRecordAsync();
            }

        }

    }
}

