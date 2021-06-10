using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

namespace VaccineSlotAvailabilityMonitor
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {

            int i = 0;

            while(i != 100)
            {
            Console.ForegroundColor = ConsoleColor.Green;

             Console.WriteLine("Vaccine availability at " + DateTime.Now + "\n");    
            var _vaccineRepositories = await VaccineRepositories();

            // foreach (var repo in _processRepositories)
            // {
            //     Console.WriteLine(repo.Name);
            //     Console.WriteLine(repo.Description);
            //     Console.WriteLine(repo.GitHubHomeUrl);
            //     Console.WriteLine(repo.Homepage);
            //     Console.WriteLine(repo.Watchers);
            //     Console.WriteLine(repo.LastPush);
            //     Console.WriteLine("Completed");
            //     Console.WriteLine();
            // }
            foreach (var repo in _vaccineRepositories.sessions)
            {
                if(repo.available_capacity > 0)
                {
                    // Console.Beep(32767,2000);
                    if(repo.available_capacity == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("White on blue.");

                    }
                    Console.WriteLine("Address : " + repo.address);
                    Console.WriteLine("Available : " + repo.available_capacity);
                    Console.WriteLine("Age Limit : " + repo.min_age_limit);
                    Console.WriteLine("Pin Code : " + repo.pincode);
                    Console.WriteLine("Vaccine : " + repo.vaccine);
                    Console.WriteLine("Fees : " + repo.fee);
                    Console.WriteLine("Date : " + repo.date);
                    Console.WriteLine();
                }
            }
            Thread.Sleep(10000);  
            i++;          

            }
            // var _processRepositories = await ProcessRepositories();
        }

        private static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }

        private static async Task<Root> VaccineRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id=307&date=11-06-2021");
            var root = await JsonSerializer.DeserializeAsync<Root>(await streamTask);
 
            // var streamTask1 = client.GetStreamAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id=307&date=10-06-2021");
            // var root1 = await JsonSerializer.DeserializeAsync<Root>(await streamTask);
 
            // var streamTask2 = client.GetStreamAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/public/findByDistrict?district_id=307&date=12-06-2021");
            // var root2 = await JsonSerializer.DeserializeAsync<Root>(await streamTask);
            // root.sessions.AddRange(root1.sessions);
            // // root.sessions.AddRange(root2.sessions);


            return root;
        }
    }
}
