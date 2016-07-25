using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var inputStream = new MemoryStream(Resources.test); // Console.OpenStandardInput();

            using (var reader = new StreamReader(inputStream, Encoding.ASCII, false, 128, true))
            {
                var timer = new Stopwatch();
                timer.Start();

                var result = new DictionaryWords().Load(reader);

                timer.Stop();
                Console.WriteLine($"Parse Dictionary Words for {timer.ElapsedMilliseconds}ms");

                timer.Restart();

                using (var outputStream = new StreamWriter(@"test.out"))
                {
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

                timer.Stop();
                Console.WriteLine($"Autocomplete Words for {timer.ElapsedMilliseconds}ms");
            }

            Console.ReadKey();
        }
    }
}
