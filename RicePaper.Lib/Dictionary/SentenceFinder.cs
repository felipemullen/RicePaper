using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RicePaper.Lib.Model;
using RicePaper.Lib.Utilities;

namespace RicePaper.Lib.Dictionary
{
    public class Core6KSentence
    {
        public string Word;
        public string Reading;
        public string Romaji;
        public string Definition;
        public string JapaneseSentence;
        public string EnglishSentence;
    }

    public class SentenceFinder
    {
        #region Constants
        private const string TATOEBA_SEARCH_API = "https://tatoeba.org/eng/sentences/search";
        #endregion

        #region Private Fields
        private const string FILE_PATH = "Resources/Content/core6k.csv";
        private readonly Regex csvRegex;

        private readonly Dictionary<string, Core6KSentence> entries;
        private readonly SimpleCache<string, TatoebaSentencePair> cache;
        #endregion

        #region Constructor
        public SentenceFinder()
        {
            csvRegex = new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");
            cache = new SimpleCache<string, TatoebaSentencePair>(maxSize: 200);

            entries = Load(FILE_PATH);
        }

        private Dictionary<string, Core6KSentence> Load(string filepath)
        {
            string fullPath = Path.Combine(Util.AppRoot, filepath);

            var all = new Dictionary<string, Core6KSentence>();
            foreach (string line in File.ReadLines(fullPath))
            {
                var parts = csvRegex.Split(line);

                var data = new Core6KSentence()
                {
                    Word = parts[0],
                    Reading = parts[1],
                    Romaji = parts[2],
                    Definition = parts[3].Trim('"'),
                    JapaneseSentence = parts[4],
                    EnglishSentence = parts[5].Trim('"')
                };

                if (all.ContainsKey(data.Word))
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine(line);
                }
                else
                {
                    all.Add(data.Word, data);
                }
            }

            return all;
        }
        #endregion

        #region Public Methods
        public Core6KSentence GetEntry(string key)
        {
            if (entries.ContainsKey(key))
            {
                return entries[key];
            }
            else
            {
                return null;
            }
        }

        public TatoebaSentencePair FindSentences(string terms)
        {
            if (cache.Contains(terms))
                return cache.Get(terms);

            try
            {
                string encodedTerms = WebUtility.UrlEncode(terms);
                string url = $"{TATOEBA_SEARCH_API}?from=jpn&to=eng&query={encodedTerms}";

                var w = new HtmlWeb();
                var doc = w.Load(url);

                var sentenceDiv = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'sentence-and-translations')]");
                var attributeValue = sentenceDiv.GetAttributeValue("ng-init", "{}");
                attributeValue = WebUtility.HtmlDecode(attributeValue);
                string parsedJson = attributeValue
                    .Replace("vm.init([], ", "[")
                    .Replace("}, [", "},")
                    .Replace("], [])", "]")
                    .Replace("], [", ",")
                    .Replace(",,", "")
                    .Replace("}])", "}]");

                var sentenceData = JsonConvert.DeserializeObject<List<TatoebaSentenceJson>>(parsedJson);

                var sentence = new TatoebaSentencePair();
                sentence.JapaneseSentence = sentenceData.Where(x => x.Lang == "jpn").FirstOrDefault()?.Text;
                sentence.EnglishSentence = sentenceData.Where(x => x.Lang == "eng").FirstOrDefault()?.Text;

                cache.Add(terms, sentence);

                return sentence;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
