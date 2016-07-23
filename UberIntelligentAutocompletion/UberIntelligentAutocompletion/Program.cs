using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputStream = File.Open(@"C:\Users\DJiN\Downloads\Тестовое задание C# разработчик\test.in", FileMode.Open, FileAccess.Read); // Console.OpenStandardInput();
            
            StreamParserPerByte.Parse(inputStream);
        }
    }
}
