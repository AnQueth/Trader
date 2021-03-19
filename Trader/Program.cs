using System;

namespace Trader
{
    class Program
    {
        private const string BTC = "BTC";
        private const string ETH = "ETH";
        private const decimal COINBASEFEE = 0.005M;
        static void Main(string[] args)
        {
            Wallet w = new Wallet(3000);

            w.Buy(BTC, 500, 50000, COINBASEFEE);

            w.Buy(ETH, 2000, 2000, COINBASEFEE);






            w.Sell(BTC, 300, 50000, COINBASEFEE);

            Console.WriteLine("sold $300 btc");



            w.Sell(BTC, 100, 50000, COINBASEFEE);


            Console.WriteLine("sold $100 btc");


            ExchangePriceChecker epc = new ExchangePriceChecker(
                new CoinMarket(),
                (p) =>
                {
                    PrintTotals(p, w);
                },
                () => w.GetCoins());

            epc.Start();

            var ep = epc.LatestPrices;
         
     
      
      

            PrintTotals(ep, w);

            PrintCurrentAndSales(ep, w);




            Console.ReadLine();

        }

        private static void PrintCurrentAndSales(ExchangePrice[] ep, Wallet w)
        {
            Console.WriteLine("Current And Sales");

            var profits = w.GetCurrentAndSales(ep);
            if (profits.Length == 0)
            {
                Console.WriteLine("EMPTY");
                return;

            }
            Console.WriteLine("Name      Amount");
            foreach (var item in profits)
                Console.WriteLine($"{item.Name,-10}{item.TotalPriceUSD,-10:F3}");

        }

        private static void PrintTotals(ExchangePrice[] ep, Wallet w)
        {
            var totals = w.GetTotal(ep);

            Console.WriteLine($"Totals AB={w.AvailableBalance}");
            Console.WriteLine($"Name      Count     USD       Coin Price");
            foreach (var item in totals)
                Console.WriteLine($"{item.Name,-10}{item.TotalCount,-10:F3}{item.TotalPriceUSD,-10:F3}{item.CurrentCoinPriceUSD}");
        }
    }
}
