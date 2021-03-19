using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trader
{
    class ExchangePriceChecker
    {
        private readonly IExchangePriceProvider _exchangePriceProvider;
        private readonly Action<ExchangePrice[]> _handler;
        private readonly Func<string[]> _coinsGetter;
        private ExchangePrice[] _latestPrices;
        private ManualResetEventSlim _started = new ManualResetEventSlim(false);
        Timer timer;
        int pollingTime;

        public ExchangePriceChecker(IExchangePriceProvider exchangePriceProvider,
            Action<ExchangePrice[]> handler, Func<string[]> coinsGetter)
        {
            _exchangePriceProvider = exchangePriceProvider;
            _handler = handler;
            _coinsGetter = coinsGetter;
            pollingTime = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            
        }

        internal ExchangePrice[] LatestPrices { get => this._latestPrices; }

        public void Start()
        {
            timer = new Timer(GetLatest, null, 0
             , Timeout.Infinite);

            _started.Wait();
        }

        private async void GetLatest(object state)
        {

            _latestPrices = await _exchangePriceProvider.Get(_coinsGetter());
            _started.Set();
            _handler(_latestPrices);
        

            timer.Change(pollingTime, Timeout.Infinite);
        }
    }
}
