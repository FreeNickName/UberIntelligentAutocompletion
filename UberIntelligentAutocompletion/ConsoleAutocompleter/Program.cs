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
            using (var input = new StreamReader(Console.OpenStandardInput(), Encoding.ASCII))
            using (var output = new StreamWriter(Console.OpenStandardOutput()))
            {
                if (input.EndOfStream)
                {
                    return;
                }
                var autocompleter = new UberIntelligentAutocompleter(input);

                var line = input.ReadLine();
                int countWords;
                if (!int.TryParse(line, out countWords))
                {
                    throw new Exception($"Count word in source is wrong: Can't parse '{line}'");
                }

                while (--countWords >= 0)
                {
                    line = input.ReadLine();
                    if (autocompleter.Complete(line, output))
                    {
                        output.WriteLine();
                    }
                }
            }
        }
    }
}
