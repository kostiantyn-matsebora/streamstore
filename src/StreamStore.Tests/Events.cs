using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamStore.Tests
{
    public class LeafEvent
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public string NullValue { get; set; }
    }

    public class BranchEvent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LeafEvent[] Leaves { get; set; }
    }

    public class RootEvent
    {
        public BranchEvent[] Branches { get; set; }

        public DateTime Timestamp { get; set; }

        public long Value { get; set; }
    }
}
