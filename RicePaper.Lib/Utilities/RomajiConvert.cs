using System.Collections.Generic;
using System.Text;

namespace RicePaper.Lib.Utilities
{
    public class RomajiConvert
    {
        #region Conversion Tables
        private static Dictionary<string, string> KanaTable = new Dictionary<string, string>()
        {
            // Hiragana
            { "あ", "a" },
            { "い", "i" },
            { "う", "u" },
            { "え", "e" },
            { "お", "o" },
            { "か", "ka" },
            { "き", "ki" },
            { "く", "ku" },
            { "け", "ke" },
            { "こ", "ko" },
            { "きゃ", "kya" },
            { "きゅ", "kyu" },
            { "きょ", "kyo" },
            { "さ", "sa" },
            { "し", "shi" },
            { "す", "su" },
            { "せ", "se" },
            { "そ", "so" },
            { "しゃ", "sha" },
            { "しゅ", "shu" },
            { "しょ", "sho" },
            { "た", "ta" },
            { "ち", "chi" },
            { "つ", "tsu" },
            { "て", "te" },
            { "と", "to" },
            { "ちゃ", "cha" },
            { "ちゅ", "chu" },
            { "ちょ", "cho" },
            { "な", "na" },
            { "に", "ni" },
            { "ぬ", "nu" },
            { "ね", "ne" },
            { "の", "no" },
            { "にゃ", "nya" },
            { "にゅ", "nyu" },
            { "にょ", "nyo" },
            { "は", "ha" },
            { "ひ", "hi" },
            { "ふ", "fu" },
            { "へ", "he" },
            { "ほ", "ho" },
            { "ひゃ", "hya" },
            { "ひゅ", "hyu" },
            { "ひょ", "hyo" },
            { "ま", "ma" },
            { "み", "mi" },
            { "む", "mu" },
            { "め", "me" },
            { "も", "mo" },
            { "みゃ", "mya" },
            { "みゅ", "myu" },
            { "みょ", "myo" },
            { "や", "ya" },
            { "ゆ", "yu" },
            { "よ", "yo" },
            { "ら", "ra" },
            { "り", "ri" },
            { "る", "ru" },
            { "れ", "re" },
            { "ろ", "ro" },
            { "りゃ", "rya" },
            { "りゅ", "ryu" },
            { "りょ", "ryo" },
            { "わ", "wa" },
            { "ゐ", "wi" },
            { "ゑ", "we" },
            { "を", "wo" },
            { "ん", "n" },
            { "が", "ga" },
            { "ぎ", "gi" },
            { "ぐ", "gu" },
            { "げ", "ge" },
            { "ご", "go" },
            { "ぎゃ", "gya" },
            { "ぎゅ", "gyu" },
            { "ぎょ", "gyo" },
            { "ざ", "za" },
            { "じ", "ji" },
            { "ず", "zu" },
            { "ぜ", "ze" },
            { "ぞ", "zo" },
            { "じゃ", "ja" },
            { "じゅ", "ju" },
            { "じょ", "jo" },
            { "だ", "da" },
            { "ぢ", "dji" },
            { "づ", "dzu" },
            { "で", "de" },
            { "ど", "do" },
            { "ぢゃ", "ja" },
            { "ぢゅ", "ju" },
            { "ぢょ", "jo" },
            { "ば", "ba" },
            { "び", "bi" },
            { "ぶ", "bu" },
            { "べ", "be" },
            { "ぼ", "bo" },
            { "びゃ", "bya" },
            { "びゅ", "byu" },
            { "びょ", "byo" },
            { "ぱ", "pa" },
            { "ぴ", "pi" },
            { "ぷ", "pu" },
            { "ぺ", "pe" },
            { "ぽ", "po" },
            { "ぴゃ","pya" },
            { "ぴゅ","pyu" },
            { "ぴょ","pyo"},
            // Katakana
            { "ア", "a" },
            { "イ", "i" },
            { "ウ", "u" },
            { "エ", "e" },
            { "オ", "o" },
            { "カ", "ka" },
            { "キ", "ki" },
            { "ク", "ku" },
            { "ケ", "ke" },
            { "コ", "ko" },
            { "キャ", "kya" },
            { "キュ", "kyu" },
            { "キョ", "kyo" },
            { "サ", "sa" },
            { "シ", "shi" },
            { "ス", "su" },
            { "セ", "se" },
            { "ソ", "so" },
            { "シャ", "sha" },
            { "シュ", "shu" },
            { "ショ", "sho" },
            { "タ","ta" },
            { "チ","chi" },
            { "ツ","tsu" },
            { "テ","te" },
            { "ト","to" },
            { "チャ","cha" },
            { "チュ","chu" },
            { "チョ","cho" },
            { "ナ","na" },
            { "ニ","ni" },
            { "ヌ","nu" },
            { "ネ","ne" },
            { "ノ","no" },
            { "ニャ","nya" },
            { "ニュ","nyu" },
            { "ニョ","nyo" },
            { "ハ","ha" },
            { "ヒ","hi" },
            { "フ","fu" },
            { "ヘ","he" },
            { "ホ","ho" },
            { "ヒャ","hya" },
            { "ヒュ","hyu" },
            { "ヒョ","hyo" },
            { "マ","ma" },
            { "ミ","mi" },
            { "ム","mu" },
            { "メ","me" },
            { "モ","mo" },
            { "ミャ","mya" },
            { "ミュ","myu" },
            { "ミョ","myo" },
            { "ヤ","ya" },
            { "ユ","yu" },
            { "ヨ","yo" },
            { "ラ","ra" },
            { "リ","ri" },
            { "ル","ru" },
            { "レ","re" },
            { "ロ","ro" },
            { "リャ","rya" },
            { "リュ","ryu" },
            { "リョ","ryo" },
            { "ワ","wa" },
            { "ヰ","wi" },
            { "ヱ","we" },
            { "ヲ","wo" },
            { "ン","n" },
            { "ガ","ga" },
            { "ギ","gi" },
            { "グ","gu" },
            { "ゲ","ge" },
            { "ゴ","go" },
            { "ギャ","gya" },
            { "ギュ","gyu" },
            { "ギョ","gyo" },
            { "ザ","za" },
            { "ジ","ji" },
            { "ズ","zu" },
            { "ゼ","ze" },
            { "ゾ","zo" },
            { "ジャ","ja" },
            { "ジュ","ju" },
            { "ジョ","jo" },
            { "ダ","da" },
            { "ヂ","ji" },
            { "ヅ","zu" },
            { "デ","de" },
            { "ド","do" },
            { "ヂャ","ja" },
            { "ヂュ","ju" },
            { "ヂョ","jo" },
            { "バ","ba" },
            { "ビ","bi" },
            { "ブ","bu" },
            { "ベ","be" },
            { "ボ","bo" },
            { "ビャ","bya" },
            { "ビュ","byu" },
            { "ビョ","byo" },
            { "パ","pa" },
            { "ピ","pi" },
            { "プ","pu" },
            { "ペ","pe" },
            { "ポ","po" },
            { "ピャ","pya" },
            { "ピュ","pyu" },
            { "ピョ","pyo" }
        };

        private static List<char> Digraphs = new List<char>()
        {
            'ゃ', 'ゅ', 'ょ', 'ャ', 'ュ', 'ョ'
        };
        #endregion

        #region Static Functions
        public static string FromKana(string kana)
        {
            System.Console.WriteLine($"translating: {kana}");

            if (string.IsNullOrWhiteSpace(kana))
                return "";

            var output = new StringBuilder();
            var input = new Queue<char>(kana);

            while (input.TryDequeue(out char current))
            {
                input.TryPeek(out char next);

                if (current == 'っ' || current == 'ッ')
                {
                    string nextRomaji = KanaTable[next.ToString()];
                    output.Append(nextRomaji[0]);
                }
                else if (current == 'ー')
                {
                    output.Append(output[output.Length - 1]);
                }
                else if (Digraphs.Contains(next))
                {
                    next = input.Dequeue();
                    string key = $"{current}{next}";
                    string roma = KanaTable[key];
                    output.Append(roma);
                }
                else
                {
                    output.Append(KanaTable[current.ToString()]);
                }
            }

            return output.ToString();
        }
        #endregion
    }
}
