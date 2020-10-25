using CycleFinder.Models;
using CycleFinder.Models.Candles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public class CsvDataService : ICandleStickRepository
    {
        private Dictionary<string, string> _symbolToPath = new Dictionary<string, string>()
        {
            {"GBPUSD", @"C:\Users\Zsolt\data\trading\data\GBPUSD=X.csv" },
            {"SPX", @"C:\Users\Zsolt\data\trading\data\SPX 5y.csv" },
        };

        public async Task<IEnumerable<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame)
        {
            string path =_symbolToPath[symbol];

            return await Task.Run(() => ReadFromCsv(path));
        }

        private IEnumerable<CandleStick> ReadFromCsv(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            var ret = new List<CandleStick>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                if (DateTime.TryParse(columns[0], out DateTime time)
                    && Double.TryParse(columns[1], out double open)
                    && Double.TryParse(columns[2], out double high)
                    && Double.TryParse(columns[3], out double low)
                    && Double.TryParse(columns[4], out double close)
                    && Double.TryParse(columns[6], out double volume))
                {
                    ret.Add(new CandleStick(time, open, high, low, close, volume));
                }
            }

            return ret;
        }

        public Task<IEnumerable<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Symbol>> ListSymbols()
        {
            return await Task.Run(() => new List<Symbol>() { new Symbol("GBPUSD", "USD"), new Symbol("SPX", "USD") });
        }
    }
}
