using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace RicePaper.Lib.Dictionary
{
    // TODO: Cache api results 

    public class JishoApi
    {
        #region Constants
        private const string BASE_URL = "https://jisho.org";
        private const string SEARCH_API = "https://jisho.org/api/v1/search/words";
        #endregion

        public JishoApi() { }

        public JishoResponse Search(string terms)
        {
            string encodedTerms = WebUtility.UrlEncode(terms);
            string url = $"{SEARCH_API}?keyword={encodedTerms}";

            var client = new HttpClient();
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var searchResponse = JsonConvert.DeserializeObject<JishoResponse>(jsonString);

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
