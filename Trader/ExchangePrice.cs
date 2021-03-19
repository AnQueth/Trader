using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    readonly struct ExchangePrice
    {
        public ExchangePrice(string name, decimal priceUSD, DateTimeOffset date)
        {
            Name = name;
            PriceUSD = priceUSD;
            Date = date;
        }

        public string Name { get; }
        public decimal PriceUSD { get; }
        public DateTimeOffset Date { get; }
    }
}
