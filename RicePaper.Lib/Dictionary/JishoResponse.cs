using System.Collections.Generic;
using Foundation;

/// <summary>
/// Several items have been commented out in order to make this
/// class ever so much slimmer in memory
/// </summary>
namespace RicePaper.Lib.Dictionary
{
    [Preserve(AllMembers = true)]
    public class JishoResponse
    {
        //public Meta meta { get; set; }
        public IEnumerable<JishoWordEntry> data { get; set; }
    }

    public class Meta
    {
        public int status { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class JishoWordEntry
    {
        //public bool is_common { get; set; }
        //public IEnumerable<string> tags { get; set; }
        public IEnumerable<JapaneseTerm> japanese { get; set; }
        public IEnumerable<Senses> senses { get; set; }
        //public Attribution attribution { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class JapaneseTerm
    {
        public string word { get; set; }
        public string reading { get; set; }
    }

    [Preserve(AllMembers = true)]
    public class Senses
    {
        public IEnumerable<string> english_definitions { get; set; }
        //public IEnumerable<object> parts_of_speech { get; set; }
        //public IEnumerable<Link> links { get; set; }
        //public IEnumerable<object> tags { get; set; }
        //public IEnumerable<object> restrictions { get; set; }
        //public IEnumerable<object> see_also { get; set; }
        //public IEnumerable<object> antonyms { get; set; }
        //public IEnumerable<object> source { get; set; }
        //public IEnumerable<object> info { get; set; }
        public IEnumerable<object> sentences { get; set; }
    }

    public class Link
    {
        public string text { get; set; }
        public string url { get; set; }
    }


    public class Attribution
    {
        public bool jmdict { get; set; }
        public bool jmnedict { get; set; }
        public object dbpedia { get; set; }
    }
}
