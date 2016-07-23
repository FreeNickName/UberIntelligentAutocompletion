using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    class HashSetTreeNode
    {
        public HashSetTreeNode(char value, int count, bool isEnd = false)
        {
            Value = value;
            IsEnd = isEnd;
            Count = count;
            Next = new Dictionary<char, HashSetTreeNode>();
        }

        public char Value { get; private set; }

        public bool IsEnd { get; set; }

        public int Count { get; private set; }

        public Dictionary<char, HashSetTreeNode> Next { get; private set; } 
    }
}
