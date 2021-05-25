using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Entities
{
    public class Statistics
    {
        public static List<WordFrequency> SortWordsByFrequency(List<string> words)
        {
            List<WordFrequency> wordFrequencies = new List<WordFrequency>();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string s in words)
            {
                if (dict.ContainsKey(s))
                {
                    dict[s]++;
                }
                else
                {
                    dict.Add(s, 1);
                }
            }
            var ditcSort = from objDict in dict orderby objDict.Value descending select objDict;
            int size = words.Count;
            foreach (KeyValuePair<string, int> kvp in ditcSort)
            {
                wordFrequencies.Add(new WordFrequency() { word = kvp.Key, frequency = (double)kvp.Value / size });
            }
            return wordFrequencies;
        }
    }

    public class WordFrequency
    {
        public string word { get; set; }
        public double frequency { get; set; }
    }
}