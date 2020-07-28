using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public class MRKToSScroll {
        public MRKToSRoleID Role { get; private set; }
        public int Count { get; set; }

        public MRKToSScroll(MRKToSRoleID role, int count) {
            Role = role;
            Count = count;
        }
    }
}
