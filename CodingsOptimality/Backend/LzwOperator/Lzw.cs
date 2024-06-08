using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingsOptimality.Backend.LzwOperator
{
    public class Lzw
    {
        public static List<int> Compress(string uncompressed)
        {
            // Build the dictionary.
            int dictSize = 256;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            string w = string.Empty;
            List<int> result = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    result.Add(dictionary[w]);
                    // Add wc to the dictionary.
                    dictionary[wc] = dictSize++;
                    w = c.ToString();
                }
            }

            // Output the code for w.
            if (!string.IsNullOrEmpty(w))
                result.Add(dictionary[w]);

            return result;
        }

        // Decompress a list of output symbols to a string.
        public static string Decompress(List<int> compressed)
        {
            // Build the dictionary.
            int dictSize = 256;
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = ((char)compressed[0]).ToString();
            compressed.RemoveAt(0);
            StringBuilder result = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                {
                    entry = dictionary[k];
                }
                else if (k == dictSize)
                {
                    entry = w + w[0];
                }

                result.Append(entry);

                // Add w+entry[0] to the dictionary.
                dictionary[dictSize++] = w + entry[0];

                w = entry;
            }

            return result.ToString();
        }
    }
}
