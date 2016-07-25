using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Diagnostics;
using UberIntelligentAutocompletion;

namespace TestAutocompletion
{
    [TestClass]
    public class MainUnitTest
    {
        [TestMethod]
        public void MainTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            using (var inputStream = new MemoryStream(Resources.test))
            using (var reader = new StreamReader(inputStream, Encoding.ASCII))
            using (var outputStream = new StreamWriter(@"test.out"))
            {
                var autocompleter = new UberIntelligentAutocompleter(reader);

                //var timestampDictionaryParsed = timer.ElapsedMilliseconds;
                //Console.WriteLine($"Parse Dictionary Words for {timestampDictionaryParsed}ms");

                var line = reader.ReadLine();
                var countWords = int.Parse(line);
                while (--countWords >= 0)
                {
                    line = reader.ReadLine();
                    if (autocompleter.Complete(line, outputStream))
                    {
                        outputStream.WriteLine();
                    }
                }
            }

            timer.Stop();
            //Console.WriteLine($"Autocomplete Words for {timer.ElapsedMilliseconds - timestampDictionaryParsed}ms");

            Assert.IsFalse(timer.ElapsedMilliseconds > 10 * 1000);
        }
    }
}
