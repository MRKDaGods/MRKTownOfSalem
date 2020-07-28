using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public interface MRKToSProxyDynamicUIListener {
        void OnDynamicButtonCreated(MRKToSDynamicButton button);
        void OnDynamicButtonDestoryed(MRKToSDynamicButton button);
    }
}
