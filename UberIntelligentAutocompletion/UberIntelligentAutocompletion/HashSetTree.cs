using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    class HashSetTree
    {
        public HashSetTree()
        {
            this.Root = new HashSet<object>();
        }

        public HashSet<object> Root { get; set; }
    }
}
