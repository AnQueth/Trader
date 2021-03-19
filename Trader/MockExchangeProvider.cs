using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    class MockExchangeProvider : IExchangePriceProvider
    {
        public Task<ExchangePrice[]> Get(string[] coins)
        {
            Random rnd = new Random();

            var md =  new ExchangePrice[]
            {
                new ExchangePrice("BTC", rnd.Next(40_000, 60_000 ), DateTimeOffset.UtcNow),
                 new ExchangePrice("ETH", rnd.Next(1_000, 3_000 ), DateTimeOffset.UtcNow)

            };

            return Task.FromResult(md);
        }
    }
}
