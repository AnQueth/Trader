using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
      readonly struct CoinTotal
    {
        public CoinTotal(string name, decimal count, decimal total, decimal currentCoinPriceUSD)
        {
            this.Name = name;
            TotalCount = count;
            TotalPriceUSD = total;
            CurrentCoinPriceUSD = currentCoinPriceUSD;
        }

        public string Name { get; }
        public decimal TotalCount { get; }
        public decimal TotalPriceUSD { get; }
        public decimal CurrentCoinPriceUSD { get; }
    }
}
