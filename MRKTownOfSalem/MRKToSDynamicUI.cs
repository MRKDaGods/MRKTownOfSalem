using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public class MRKToSDynamicUI {
        Dictionary<int, MRKToSDynamicButton> m_Buttons;

        public MRKToSDynamicUI() {
            m_Buttons = new Dictionary<int, MRKToSDynamicButton>();
        }

        public MRKToSDynamicButton CreateDynamicButton(MRKToSPlayer owner, string name, Action<MRKToSDynamicButton, bool> stateDel = null) {
            MRKToSDynamicButton button = new MRKToSDynamicButton(owner, name, stateDel);
            m_Buttons[button.ID] = button;

            //handle button creation
            owner.Match.UIListener.OnDynamicButtonCreated(button);

            return button;
        }

        public void DestroyDynamicButton(int id) {
            if (!m_Buttons.ContainsKey(id))
                throw new ArgumentException($"Button {id} does not exist");

            m_Buttons.Remove(id);
        }
    }
}
