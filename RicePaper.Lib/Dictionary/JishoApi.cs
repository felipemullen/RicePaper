using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace RicePaper.Lib.Dictionary
{
    public class JishoApi
    {
        #region Constants
        private const string BASE_URL = "https://jisho.org";
        private const string SEARCH_API = "https://jisho.org/api/v1/search/words";
        #endregion

        #region Private Fields
        private readonly SimpleCache<string, JishoResponse> cache;
        #endregion

        #region Constructor
        public JishoApi()
        {
            cache = new SimpleCache<string, JishoResponse>(maxSize: 200);
        }
        #endregion

        public JishoResponse Search(string terms)
        {
            if (cache.Contains(terms))
                return cache.Get(terms);

            string encodedTerms = WebUtility.UrlEncode(terms);
            string url = $"{SEARCH_API}?keyword={encodedTerms}";

            var client = new HttpClient();
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var searchResponse = JsonConvert.DeserializeObject<JishoResponse>(jsonString);

                cache.Add(terms, searchResponse);

                return searchResponse;
            }
            else
            {
                var message = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Bad response from jisho.org: {0} - {1}", response.StatusCode, message);

                return null;
            }
        }
    }
}
