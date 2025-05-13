using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CompanyLookup.Models;


namespace CompanyLookup.Services
{
    public class StatsService
    {
        //lese csv fil
        public async Task<List<FirmaOutput>> LoadAsync(string path)
        {
            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, cfg);

            var list = new List<FirmaOutput>();

            await foreach (var rec in csv.GetRecordsAsync<FirmaOutput>())
                list.Add(rec);

            return list;
        }


        //Calculate 
        public void CalculateAndPrint(List<FirmaOutput> rows)
        {
            int total = rows.Count;
            Console.WriteLine($"\nTotalt: {total} rader\n");

            /* Antall per Status */
            Console.WriteLine("Status-fordeling:");
            rows.GroupBy(r => r.Status)
                .OrderByDescending(g => g.Count())
                .ToList()
                .ForEach(g => Console.WriteLine($"  {g.Key,-15}: {g.Count()}"));

            /* Organisasjonsform */
            Console.WriteLine("\nOrganisasjonsformer (%):");
            rows.Where(r => !string.IsNullOrWhiteSpace(r.OrganisasjonsformKode))
                .GroupBy(r => r.OrganisasjonsformKode!)
                .OrderByDescending(g => g.Count())
                .ToList()
                .ForEach(g =>
                {
                    double pct = g.Count() * 100.0 / total;
                    Console.WriteLine($"  {g.Key,-4}: {g.Count(),4}  ({pct,5:F1} %)");
                });

            /* fordeling av antall ansatte */
            Console.WriteLine("\nAnsattgrupper:");
            var buckets = rows.GroupBy(r => Bucket(r.AntallAnsatte));
            foreach (var label in new[] { "0", "1-9", "10-49", "50+" })
            {
                int count = buckets.FirstOrDefault(g => g.Key == label)?.Count() ?? 0;
                double pct = count * 100.0 / total;
                Console.WriteLine($"  {label,-6}: {count,4}  ({pct,5:F1} %)");
            }
        }
         
        private static string Bucket(int? ansatte) => ansatte switch
        {
            0 => "0",
            >= 1 and <= 9 => "1-9",
            >= 10 and <= 49 => "10-49",
            _ => "50+"
        };
    }
}