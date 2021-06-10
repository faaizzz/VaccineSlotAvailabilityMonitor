using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using VaccineSlotAvailabilityMonitor.Extentions;
using System.Linq;

namespace VaccineSlotAvailabilityMonitor
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static int tableWidth = 140;

        static async Task Main(string[] args)
        {

            int i = 0;

            while (i != 100)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.Clear();
                Console.WriteLine("Vaccine availability at " + DateTime.Now + "\n");
                PrintLine();
                PrintRow("Pin Code", "Total | Dose 1 | Dose 2", "Date", "Age Limit", "Address");
                PrintLine();
                var _vaccineRepositories = await VaccineRepositories();

                foreach (var repo in _vaccineRepositories.sessions)
                {
                    if (repo.available_capacity > 0)
                    {
                        if (repo.pincode == 683565)
                        {
                            Console.Beep(32767, 2000);

                        }
                        if (repo.available_capacity == 1)
                        {
                        }

                        PrintRow(repo.pincode.ToString(),
                         repo.available_capacity.ToString() + " | " + repo.available_capacity_dose1.ToString() + " | " + repo.available_capacity_dose2.ToString(),
                         repo.date.ToString(),
                           repo.min_age_limit.ToString(),
                           repo.address);
                        PrintLine();

                        // Console.WriteLine("Address : " + repo.address);
                        // Console.WriteLine("Total Available : " + repo.available_capacity);
                        // Console.ForegroundColor = ConsoleColor.Yellow;
                        // Console.WriteLine("Dose 1 : " + repo.available_capacity_dose1);
                        // Console.ForegroundColor = ConsoleColor.Green;
                        // Console.WriteLine("Dose 2 : " + repo.available_capacity_dose2);
                        // Console.WriteLine("Age Limit : " + repo.min_age_limit);
                        // Console.WriteLine("Pin Code : " + repo.pincode);
                        // Console.WriteLine("Vaccine : " + repo.vaccine);
                        // Console.WriteLine("Fees : " + repo.fee);
                        // Console.WriteLine("Date : " + repo.date);
                        // Console.WriteLine();
                    }
                }
                Thread.Sleep(10000);
                i++;

            }
            // var _processRepositories = await ProcessRepositories();
        }
        private static async Task<Root> VaccineRepositories()
        {
            Root Result = new Root();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTaskToday = client.GetStreamAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id=307&date=10-06-2021");
            var today = await JsonSerializer.DeserializeAsync<Root>(await streamTaskToday);

            var streamTaskTomorrow = client.GetStreamAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id=307&date=11-06-2021");
            var tomorrow = await JsonSerializer.DeserializeAsync<Root>(await streamTaskTomorrow);


            var streamTaskDayAfterTomorrow = client.GetStreamAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id=307&date=12-06-2021");
            var dayAfterTomorrow = await JsonSerializer.DeserializeAsync<Root>(await streamTaskDayAfterTomorrow);

            Result.sessions = tomorrow.sessions.Join(today.sessions);
            Result.sessions = Result.sessions.Join(dayAfterTomorrow.sessions).OrderByDescending(x=>x.available_capacity).ToList();
            return Result;
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
