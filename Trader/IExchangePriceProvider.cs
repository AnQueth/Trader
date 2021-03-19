using System.Threading.Tasks;

namespace Trader
{
    interface IExchangePriceProvider
    {
        Task<ExchangePrice[]> Get(string[] coins);
    }
}