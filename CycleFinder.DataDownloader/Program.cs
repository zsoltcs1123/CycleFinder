using CycleFinder.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using CycleFinder.Models;
using CsvHelper;
using System.Globalization;

namespace CycleFinder.DataDownloader
{
    internal class Program
    {
        private readonly static string[] _timeFrames = { "H1", "Daily", "H4"};

        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });
            ILogger<BinanceDataService> logger = loggerFactory.CreateLogger<BinanceDataService>();

            var dataService = new BinanceDataService(logger);

            var symbols = (await dataService.ListSymbols()).ToList();

/*            Console.Write("Enter symbol:");
            var symbol = Console.ReadLine();

            Console.Write("Enter start date:");
            var startDateStr = Console.ReadLine();

            Console.Write("Enter timeframe:");
            var timeFrame = Console.ReadLine()*/;

            //debug
            var symbol = "BTCUSDT";
            var timeFrame = "H1";


            if (string.IsNullOrEmpty(symbol) || !symbols.Any(_ => _.Name == symbol))
            {
                Console.WriteLine("No such symbol");
                return;
            }

            if (string.IsNullOrEmpty(timeFrame) || !_timeFrames.Contains(timeFrame) || !Enum.TryParse(timeFrame, out TimeFrame timeFrameEnum))
            {
                Console.WriteLine("Invalid timeframe");
                return;
            }

            var data = (await dataService.GetAllData(symbol, timeFrameEnum)).ToList();

            using (var writer = new StreamWriter($"{symbol}_{timeFrame}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }

        }
    }
}