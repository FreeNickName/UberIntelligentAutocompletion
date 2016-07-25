using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    /// <summary>
    /// Менеджер дополнения слов по словарю.
    /// </summary>
    public class UberIntelligentAutocompleter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary">Словарь.</param>
        /// <param name="maxCompleted">Максимальное кол-во слов в подсказке.</param>
        public UberIntelligentAutocompleter(IDictionary<char, SymbolNode> dictionary, int maxCompleted = 10)
        {
            Dictionary = dictionary;
            MaxCompleted = maxCompleted;
        }

        /// <summary>
        /// Словарь.
        /// </summary>
        public IDictionary<char, SymbolNode> Dictionary { get; private set; }

        /// <summary>
        /// Максимальное кол-во слов в подсказке.
        /// </summary>
        public int MaxCompleted { get; set; }

        /// <summary>
        /// Автодополнить строку.
        /// </summary>
        /// <param name="line">Строка.</param>
        /// <param name="output">Вывод.</param>
        /// <returns></returns>
        public bool Autocomplete(string line, StreamWriter output)
        {
            if (null == output) throw new ArgumentNullException(nameof(output));

            var hashSet = Dictionary;
            SymbolNode node = null;

            for (int i = 0; i < line.Length; ++i)
            {
                var symbol = line[i];

                if (!hashSet.TryGetValue(symbol, out node))
                {
                    return false;
                }
                hashSet = node.Next;
            }

            var limit = MaxCompleted;
            if (node.IsEnd)
            {
                output.WriteLine(line);
                --limit;
            }

            FindWords(node.Next.OrderByDescending(o => o.Value.Weight).ThenBy(n => n.Key), line, ref limit, output);
            return true;
        }

        /// <summary>
        /// Выводим самые распостраненные слова.
        /// </summary>
        /// <param name="wordCompletion">Доступные завершения слов.</param>
        /// <param name="startWord">Строка, которую необходимо дополнить.</param>
        /// <param name="limit">Счетчик выводимых дополнений.</param>
        /// <param name="output">Вывод.</param>
        private void FindWords(IEnumerable<KeyValuePair<char, SymbolNode>> wordCompletion, string startWord, ref int limit, StreamWriter output)
        {
            foreach (var symbolNode in wordCompletion)
            {
                if (limit <= 0)
                {
                    break;
                }

                if (symbolNode.Value.IsEnd)
                {
                    --limit;
                    output.WriteLine(startWord + symbolNode.Key);
                }
                if (symbolNode.Value.Next.Count == 0)
                {
                    continue;
                }
                var newStart = (string)startWord.Clone() + symbolNode.Key;
                FindWords(symbolNode.Value.Next.OrderByDescending(o => o.Value.Weight).ThenBy(n => n.Key), newStart, ref limit, output);
            }
        }
    }
}
