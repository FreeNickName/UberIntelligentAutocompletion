using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    /// <summary>
    /// Информация о положении символа в слове.
    /// </summary>
    public class SymbolEntry
    {
        public SymbolEntry(char value)
        {
            Value = value;
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public char Value { get; private set; }

        /// <summary>
        /// Этот символ завершает слово.
        /// </summary>
        public bool IsEnd { get; set; } = false;

        /// <summary>
        /// Кол-во слов, в которых задействован символ.
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// Вес.
        /// </summary>
        public long Weight { get; set; } = 0;

        /// <summary>
        /// Следующие символы.
        /// </summary>
        public TableSymbols Next { get; private set; } = new TableSymbols();
    }
}
