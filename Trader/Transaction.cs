using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    readonly struct Transaction
    {
        public Transaction(string name, decimal count, decimal priceUSD, bool isSale, decimal coinPrice, decimal fee)
        {
            this.Name = name;
            this.Count = count;
            this.PriceUSD = priceUSD;
            this.IsSale = isSale;
            this.CoinPrice = coinPrice;
            this.Fee = fee;
            this.Date = DateTimeOffset.UtcNow;
        }

        public string Name { get; }
        public decimal Count { get; }

        public bool IsSale { get; }
        public decimal CoinPrice { get; }
        public decimal Fee { get; }
        public DateTimeOffset Date { get; }
        public decimal PriceUSD { get; }

     
    }
}
