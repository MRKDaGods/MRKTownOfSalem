using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public class MRKToSUtilities {
        public static bool Bound(long val, long dif, out long realValue, long deg = 1000L) {
            long stationary = val;
            while (stationary >= deg)
                stationary -= deg;

            long offset = stationary;
            if (offset >= deg / 2L)
                offset = deg;
            else
                offset = 0L;

            realValue = val - stationary + offset;
            return Math.Abs(realValue - val) <= dif;
        }
    }
}
