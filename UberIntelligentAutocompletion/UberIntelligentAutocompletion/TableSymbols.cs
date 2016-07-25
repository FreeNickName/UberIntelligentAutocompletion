using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    /// <summary>
    /// Таблица символов.
    /// </summary>
    public class TableSymbols : Dictionary<char, SymbolEntry>
    {
        /// <summary>
        /// Загрузить данные в таблицу.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static TableSymbols Load(StreamReader reader)
        {
            if (null == reader) throw new ArgumentNullException(nameof(reader));
            if (reader.EndOfStream) throw new ArgumentException("EndOfStream", nameof(reader));

            var line = reader.ReadLine();

            int countWords;
            if (!int.TryParse(line, out countWords))
            {
                throw new Exception($"Count word in source is wrong: Can't parse '{line}'");
            }

            var result = new TableSymbols();
            while (--countWords >= 0)
            {
                line = reader.ReadLine();
                var tokens = line.Split(' ');
                var word = tokens[0];

                int weight;
                if (!int.TryParse(tokens[1], out weight))
                {
                    throw new Exception($"Weight word in line '{line}' is wrong");
                }

                var isNew = false;
                SymbolEntry entry = null;
                var tableSymbols = result;
                foreach (var symbol in word)
                {
                    if (isNew || !tableSymbols.TryGetValue(symbol, out entry))
                    {
                        tableSymbols.Add(symbol, entry = new SymbolEntry(symbol));
                        isNew = true;
                    }

                    entry.Weight += weight;
                    entry.Count++;
                    tableSymbols = entry.Next;
                }
                entry.IsEnd = true;
            }
            return result;
        }
    }
}
