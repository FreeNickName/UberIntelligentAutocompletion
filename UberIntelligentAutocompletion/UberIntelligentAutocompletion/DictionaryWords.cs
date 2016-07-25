using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    /// <summary>
    /// Словарь символов.
    /// </summary>
    public class DictionaryWords
    {
        /// <summary>
        /// Содержимое словаря.
        /// </summary>
        public IDictionary<char, SymbolNode> Data { get; private set; } = new Dictionary<char, SymbolNode>();

        /// <summary>
        /// Загрузить данные в словарь.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public IDictionary<char, SymbolNode> Load(StreamReader reader)
        {
            if (null == reader) throw new ArgumentNullException(nameof(reader));

            if (reader.EndOfStream)
            {
                return Data;
            }
            var line = reader.ReadLine();

            int countWords;
            if (!int.TryParse(line, out countWords))
            {
                throw new Exception($"Count word in source is wrong: Can't parse '{line}'");
            }

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
                SymbolNode node = null;
                var tableSymbols = Data;
                foreach (var symbol in word)
                {
                    if (isNew || !tableSymbols.TryGetValue(symbol, out node))
                    {
                        tableSymbols.Add(symbol, node = new SymbolNode(symbol));
                        isNew = true;
                    }

                    node.Weight += weight;
                    node.Count++;
                    tableSymbols = node.Next;
                }
                node.IsEnd = true;
            }
            return Data;
        }
    }
}
