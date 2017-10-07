using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Generates a list of Random words
    /// </summary>
    public class RandomWords
    {
        public static string WordsFileLocation = @"..\words.json";

        private static Random r = new Random();
        private static string[] _words;
        public static string[] Words
        {
            get
            {
                if (_words == null)
                    _words = LoadWords(WordsFileLocation);
                return _words;
            }
        }

        /// <summary>
        /// Gets a sentence the specified number of random words long
        /// </summary>
        /// <param name="numberOfWords">How long the sentence should be</param>
        public static string GetWords(int numberOfWords)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < numberOfWords; i++)
            {
                builder.Append(Words[r.Next(0, Words.Length)] + " ");
            }

            return builder.ToString();
        }

        //Deserializes word list from a file
        private static string[] LoadWords(string path)
        {
            return JsonConvert.DeserializeObject<IEnumerable<string>>(File.ReadAllText(path)).ToArray();
        }

        //Converts a dictionary file to just a word list
        public static void ConvertDictionary()
        {
            //From https://github.com/adambom/dictionary/blob/master/dictionary.json
            var entries = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(@"C:\temp\dictionary.json"));
            var words = entries.Select(e => e.Key);
            var output = JsonConvert.SerializeObject(words);
            File.WriteAllText(@"C:\temp\words.json", output);
        }

    }
}
