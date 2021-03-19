using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    class CoinMarket : IExchangePriceProvider
    {
        private const string API_KEY = @"2539de19-e92a-419a-83a4-842b57eb54e4";
        public async Task<ExchangePrice[]> Get(string[] coins)
        {
            //            //https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest

            //         string resp =   @"{
            //""data"": {
            //""1"": {
            //""id"": 1,
            //""name"": ""Bitcoin"",
            //""symbol"": ""BTC"",
            //""slug"": ""bitcoin"",
            //""is_active"": 1,
            //""is_fiat"": 0,
            //""circulating_supply"": 17199862,
            //""total_supply"": 17199862,
            //""max_supply"": 21000000,
            //""date_added"": ""2013-04-28T00:00:00.000Z"",
            //""num_market_pairs"": 331,
            //""cmc_rank"": 1,
            //""last_updated"": ""2018-08-09T21:56:28.000Z"",
            //""tags"": [
            //""mineable""
            //],
            //""platform"": null,
            //""quote"": {
            //""USD"": {
            //""price"": 6602.60701122,
            //""volume_24h"": 4314444687.5194,
            //""percent_change_1h"": 0.988615,
            //""percent_change_24h"": 4.37185,
            //""percent_change_7d"": -12.1352,
            //""percent_change_30d"": -12.1352,
            //""market_cap"": 113563929433.21645,
            //""last_updated"": ""2018-08-09T21:56:28.000Z""
            //}
            //}
            //}
            //},
            //""status"": {
            //""timestamp"": ""2021-03-19T20:56:10.191Z"",
            //""error_code"": 0,
            //""error_message"": """",
            //""elapsed"": 10,
            //""credit_count"": 1
            //}
            //}
            //";

            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest");



            URL.Query = "symbol=" + String.Join(',', coins);

            HttpClient httpClient = new HttpClient();


            httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");


            var r = await httpClient.GetAsync(URL.ToString());
            var resp = await r.Content.ReadAsStringAsync();

            List<ExchangePrice> prices = new List<ExchangePrice>();
            JObject j = JObject.Parse(resp);
            foreach (var item in j["data"])
            {
                prices.Add(new ExchangePrice(item.First["symbol"].ToString(),
                    item.First["quote"]["USD"]["price"].ToObject<decimal>(),
                    DateTimeOffset.Parse(item.First["quote"]["USD"]["last_updated"].ToString())
                    ));
            }


            return prices.ToArray();
        }
    }
}
