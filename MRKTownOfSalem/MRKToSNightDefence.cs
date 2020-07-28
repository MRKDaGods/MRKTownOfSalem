using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public enum MRKToSDefenceType {
        None = 0,
        Basic = 1,
        Powerful = 2,
        Unstoppable = 3
    }

    public class MRKToSNightDefence {
        public MRKToSDefenceType DefenceType { get; private set; }
        public MRKToSPlayer DefenceProvider { get; private set; }

        public MRKToSNightDefence(MRKToSDefenceType defenceType, MRKToSPlayer provider) {
            DefenceType = defenceType;
            DefenceProvider = provider;
        }
    }
}
