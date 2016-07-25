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
        public UberIntelligentAutocompleter(TableSymbols dictionary, int maxCompleted = 10)
        {
            Dictionary = dictionary;
            MaxCompleted = maxCompleted;
        }

        public UberIntelligentAutocompleter(StreamReader dictionaryReader, int maxCompleted = 10)
        {
            Dictionary = TableSymbols.Load(dictionaryReader);
            MaxCompleted = maxCompleted;
        }

        /// <summary>
        /// Словарь.
        /// </summary>
        public TableSymbols Dictionary { get; private set; }

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
        public bool Complete(string line, TextWriter output)
        {
            if (null == output) throw new ArgumentNullException(nameof(output));
            if (String.IsNullOrWhiteSpace(line)) throw new ArgumentException("Line is empty", nameof(line));

            var hashSet = Dictionary;
            SymbolEntry entry = null;
            foreach (var symbol in line)
            {
                if (!hashSet.TryGetValue(symbol, out entry))
                {
                    return false;
                }
                hashSet = entry.Next;
            }

            var limit = MaxCompleted;
            if (entry.IsEnd)
            {
                output.WriteLine(line);
                --limit;
            }

            return FindWords(entry.Next.OrderByDescending(o => o.Value.Weight).ThenBy(n => n.Key), line, ref limit, output) < MaxCompleted;
        }

        /// <summary>
        /// Выводим самые распостраненные слова.
        /// </summary>
        /// <param name="wordCompletion">Доступные завершения слов.</param>
        /// <param name="startWord">Строка, которую необходимо дополнить.</param>
        /// <param name="limit">Счетчик выводимых дополнений.</param>
        /// <param name="output">Вывод.</param>
        /// <returns>Оставшийся лимит.</returns>
        private int FindWords(IEnumerable<KeyValuePair<char, SymbolEntry>> wordCompletion, string startWord, ref int limit, StreamWriter output)
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
            return limit;
        }
    }
}
