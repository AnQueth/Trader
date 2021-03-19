using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    class Wallet
    {
        private List<Transaction> transactions = new List<Transaction>();

        public decimal AvailableBalance { get; private set; }

        private readonly Action<CoinTotal[]> _walletUpdated;

        public Wallet(decimal totalUSD )
        {
            AvailableBalance = totalUSD;
            
        }

        public string[] GetCoins()
        {
            var res = (from o in transactions
                       where !o.IsSale
                       
                       select o.Name
                       ).Distinct().ToArray();

            return res;
        }

        public void Buy(string name, decimal priceUSD, decimal coinPrice, decimal fee)
        {
          
            if (  priceUSD  > AvailableBalance)
                throw new ArgumentOutOfRangeException(nameof(priceUSD));
         
            decimal count =   (priceUSD - priceUSD * fee) / coinPrice;

            AvailableBalance -=  priceUSD ;

            transactions.Add(new Transaction(name, count, priceUSD, false, coinPrice, fee));

 
        }

        public void Sell(string name,  decimal priceUSD, decimal coinPrice, decimal fee )
        {
      
            var ct = GetTotal(new[] { new ExchangePrice(name, coinPrice, DateTimeOffset.UtcNow) });

            if(ct[0].TotalPriceUSD  < priceUSD)
            {
                throw new ArgumentOutOfRangeException(nameof(coinPrice));
            }
            
            decimal count = priceUSD / coinPrice;
            AvailableBalance +=  priceUSD   - priceUSD * fee;
            

            transactions.Add(new Transaction(name, count, priceUSD, true, coinPrice, fee));
        }

        public CoinTotal[] GetTotal(IEnumerable<ExchangePrice> prices)
        {

            var res = from o in prices
                      join c in transactions on o.Name equals c.Name                   
                      group c by new { c.Name, o.PriceUSD } into coinsByName
                      select new CoinTotal(coinsByName.Key.Name, 
                      coinsByName.Where(z=>!z.IsSale).Sum(z => z.Count) +
                         (coinsByName.Where(z => z.IsSale).Sum(z => z.Count) * -1)
                      ,
                      coinsByName.Where(z=>!z.IsSale).Sum(z => z.Count) * coinsByName.Key.PriceUSD +
                      ( coinsByName.Where(z => z.IsSale).Sum(z => z.Count) * coinsByName.Key.PriceUSD * -1)
                      , coinsByName.Key.PriceUSD);


            return res.ToArray();


        }

        public CoinTotal[] GetCurrentAndSales(IEnumerable<ExchangePrice> prices)
        {
            var sumSales = from o in transactions
                               where o.IsSale
                               group o by o.Name into coinsByName
                              
                               select new CoinTotal
                               (coinsByName.Key,
                                -1,
                                    coinsByName.Sum(z =>   z.PriceUSD ), 0);

            var holdings = GetTotal(prices).ToList();


            List<CoinTotal> l = new List<CoinTotal>();
            foreach(var item in sumSales)
            {

                if(holdings.Any(z=>z.Name == item.Name))
                {
                    var p = holdings.First(z => z.Name == item.Name).TotalPriceUSD;

                    var t = item.TotalPriceUSD + p;

                    l.Add(new CoinTotal(item.Name, -1, t, p));
                    holdings.RemoveAll(z => z.Name == item.Name);
                }
                else
                {
                    l.Add(item);
                }
            }

            foreach (var item in holdings)
                l.Add(item);

            return l.ToArray();



    }
}
}
