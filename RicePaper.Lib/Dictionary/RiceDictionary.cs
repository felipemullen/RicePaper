using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RicePaper.Lib.Model;
using RicePaper.Lib.Utilities;

namespace RicePaper.Lib.Dictionary
{
    public class RiceDictionary : ListIterator<string>
    {
        #region Private Fields
        private readonly JishoApi jishoApi;
        private readonly SentenceFinder sentenceFinder;
        private HashSet<string> blacklist;
        #endregion

        #region Properties
        private string blacklistPath { get { return Path.Combine(Util.AppContainer, "_rp_blacklist.txt"); } }
        #endregion

        #region Constructor
        public RiceDictionary() : base()
        {
            this.jishoApi = new JishoApi();
            this.sentenceFinder = new SentenceFinder();
            LoadBlacklist();
        }
        #endregion

        #region Public Methods
        public TextDetails CurrentDefinition(AppSettings settings)
        {
            string currentWord = CurrentItem;

            CacheNextWords(count: 10);

            var sentence = sentenceFinder.GetEntry(currentWord);
            if (sentence != null)
            {
                return RemoveDisabledOptions(settings, TextDetails.FromSentence(sentence));
            }
            else
            {
                var textDetails = new TextDetails();

                try
                {
                    var data = jishoApi.Search(currentWord);
                    textDetails = TextDetails.FromJisho(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to get get Jisho data: {0}", e.Message);
                }

                try
                {
                    textDetails.Romaji = RomajiConvert.FromKana(textDetails.Furigana);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to convert kana to romaji: {0}", e.Message);
                }

                if (settings.TextOptions.EnglishSentence || settings.TextOptions.JapaneseSentence)
                {
                    try
                    {
                        var tatoebaSentence = sentenceFinder.FindSentences(currentWord);
                        if (sentence != null)
                        {
                            textDetails.JapaneseSentence = tatoebaSentence.JapaneseSentence;
                            textDetails.EnglishSentence = tatoebaSentence.EnglishSentence;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unable to find sentences: {0}", e.Message);
                    }
                }

                return RemoveDisabledOptions(settings, textDetails);
            }
        }

        public void Load(WordListSelection option)
        {
            var filePath = AppSettings.GetFilePath(option);
            Load(filePath);
        }
        #endregion

        #region Blacklist
        private void LoadBlacklist()
        {
            blacklist = new HashSet<string>();

            if (File.Exists(blacklistPath) == false)
                File.WriteAllText(blacklistPath, string.Empty);

            ReloadBlacklist();
        }

        public void ReloadBlacklist()
        {
            try
            {
                blacklist.Clear();
                var lines = File.ReadAllLines(blacklistPath);
                foreach (var line in lines)
                {
                    blacklist.Add(line);
                }
            }
            catch (Exception)
            {
                // In the event something bad happens, rewrite the file
                File.WriteAllText(blacklistPath, string.Empty);
            }
        }

        public void AddToBlacklist(string word)
        {
            if (blacklist.Contains(word) == false)
            {
                blacklist.Add(word);
                Task.Run(() =>
                {
                    File.AppendAllLines(blacklistPath, new string[] { word });
                });
            }
        }

        public bool BlacklistContains(string word)
        {
            return blacklist.Contains(word);
        }

        public void OpenBlacklistInEditor()
        {
            Util.OpenFileFromPath(blacklistPath);
        }
        #endregion

        #region ListIterator Implementation
        protected override IList<string> LoadData(string path)
        {
            var wordList = File.ReadAllLines(path);

            return wordList;
        }
        #endregion

        #region Private Helpers
        private TextDetails RemoveDisabledOptions(AppSettings settings, TextDetails details)
        {
            return new TextDetails()
            {
                Kanji = settings.TextOptions.Kanji ? details.Kanji : string.Empty,
                Furigana = settings.TextOptions.Furigana ? details.Furigana : string.Empty,
                EnglishSentence = settings.TextOptions.EnglishSentence ? details.EnglishSentence : string.Empty,
                JapaneseSentence = settings.TextOptions.JapaneseSentence ? details.JapaneseSentence : string.Empty,
                Definition = settings.TextOptions.Definition ? details.Definition : string.Empty,
                Romaji = settings.TextOptions.Romaji ? details.Romaji : string.Empty
            };
        }

        private void CacheNextWords(int count)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    var item = currentList[Index + i];
                    jishoApi.Search(item);
                    sentenceFinder.FindSentences(item);
                }
            });
        }
        #endregion
    }
}
