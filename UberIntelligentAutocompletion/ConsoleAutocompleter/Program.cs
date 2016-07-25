using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberIntelligentAutocompletion;

namespace ConsoleAutocompleter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(Console.OpenStandardInput(), Encoding.ASCII))
            {
                var result = new DictionaryWords().Load(reader);
                using (var outputStream = new StreamWriter(Console.OpenStandardOutput()))
                {
                    if (reader.EndOfStream)
                    {
                        return;
                    }
                    var autocompleter = new UberIntelligentAutocompleter(result);

                    var line = reader.ReadLine();
                    var countWords = int.Parse(line);
                    while (--countWords >= 0)
                    {
                        line = reader.ReadLine();
                        if (autocompleter.Autocomplete(line, outputStream))
                        {
                            outputStream.WriteLine();
                        }
                    }
                }
            }
        }
    }
}
