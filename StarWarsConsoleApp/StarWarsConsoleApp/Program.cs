using StarWarsAPI;
using StarWarsAPI.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static StarWarsConsoleApp.Enums;

namespace StarWarsConsoleApp
{
    public class Program
    {
        public static StarWarsEntityList<Starship> _lstStarships = new StarWarsEntityList<Starship>();
        public const double daysInYear = 365.25;

        public static void Main(string[] args)
        {
            string stops = string.Empty;
            string input;
            double inputDist = 0.0;

            // Get data inputs
            Console.WriteLine("StarWars application\n");
            Console.WriteLine("Find out how many stops are needed to resupply the starships over a specified distance.\n To exit, type 'x' and press Enter. \n");
            Console.WriteLine("\b--------------------------------------------------");
            Console.Write("Please enter a value(MGLT): ");

            input = Console.ReadLine();

            // Check if a value was entered
            if (string.IsNullOrWhiteSpace(Convert.ToString(input)) == false)
            {
                try
                {
                    inputDist = Convert.ToDouble(input);

                    if (inputDist > -1) //  Check input is not below 0
                    {
                        Console.WriteLine("\nResults: \n");
                        GetStarships(stops, inputDist).Wait();

                        Console.WriteLine("\n__________________________________________________\n");
                    }
                    else
                    {
                        Console.WriteLine("Value cannot be lower than 0");
                        Console.WriteLine("\n__________________________________________________\n");
                    }

                }
                catch (FormatException f)
                {
                    // Check if format exception thrown was for 'X' entered -- close application
                    if (Convert.ToString(input) == "x")
                    {
                        Console.WriteLine("\nApplication will now close");
                        Thread.Sleep(1500);
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a numberic value\n");
                        Console.WriteLine("\n__________________________________________________\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured. Please try again.");
                }

                Main(args);
            }

            else
                Main(args);
        }

        public static async Task<StarWarsEntityList<Starship>> GetStarships(string stops, double inputDist)
        {
            var api = new StarWarsAPIClient();

            // Get first page of all starships available
            StarWarsEntityList<Starship> _lstStarships = await api.GetAllStarship("1");

            // Get the number of pages
            int totalPages = Convert.ToInt32(Math.Ceiling(((double)_lstStarships.count / _lstStarships.results.Count())));

            // While loop to filter through all pages of data returned (data returns results in groups of 10)
            for (int pageNum = 1; pageNum < totalPages + 1; pageNum++)
            {

                if (pageNum != 1) // Skip recalling page 1. Only needs recallinging for changing to pages that follow
                {
                    _lstStarships = await api.GetAllStarship(pageNum.ToString());
                }


                foreach (Starship s in _lstStarships.results)
                {
                    stops = CalculateStops(s.consumables, s.MGLT, inputDist);

                    Console.WriteLine(string.Format("{0}: {1}", s.name, stops));
                }
            }

            return _lstStarships;
        }

        public static string CalculateStops(string consumables, string mglt, double dist)
        {
            string stops = "";
            double sum = 0;

            string str = new String(consumables.Where(Char.IsDigit).ToArray()); // Get numeric values from consumables if exist

            // Check if any required values missing. If yes returns "Unknown".
            if (mglt.ToLower().Contains("unknown") || consumables.ToLower().Contains("unknown"))
            {
                stops = "Unknown";
            }
            else
            {
                sum = dist / double.Parse(mglt);

                // Filter by consumables and calculate stops according to timespan
                if (consumables.Contains("year"))
                {
                    sum = Math.Floor(sum / ((double.Parse(str) * daysInYear) * (double)TimeSets.HoursInDay));
                }

                else if (consumables.Contains("month"))
                {
                    sum = Math.Floor(sum / (((daysInYear / (double)TimeSets.MonthsInYear) * double.Parse(str)) * (double)TimeSets.HoursInDay));
                }

                else if (consumables.Contains("week"))
                {
                    sum = Math.Floor(sum / (((daysInYear / (double)TimeSets.WeeksInYear) * double.Parse(str)) * (double)TimeSets.HoursInDay));
                }

                else if (consumables.Contains("day"))
                {
                    sum = Math.Floor(sum / (double.Parse(str) * (double)TimeSets.HoursInDay));
                }

                stops = Convert.ToString(sum);
            }
            return stops;
        }
    }
}