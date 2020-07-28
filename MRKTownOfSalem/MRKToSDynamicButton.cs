using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public class MRKToSDynamicButton {
        static int ms_GlobalId;

        public MRKToSPlayer Associated { get; private set; }
        public string Name { get; private set; }
        public bool State { get; private set; }
        public Action<MRKToSDynamicButton, bool> OnDynamicButtonStateChanged { get; set; }
        public int ID { get; private set; }

        public MRKToSDynamicButton(MRKToSPlayer associated, string name, Action<MRKToSDynamicButton, bool> buttonStateDel = null) {
            Associated = associated;
            Name = name;
            OnDynamicButtonStateChanged = buttonStateDel;

            ID = ms_GlobalId++;
        }
    }
}
