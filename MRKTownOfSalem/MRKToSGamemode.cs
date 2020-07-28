using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    /// <summary>
    /// TownOfSalem match's gamemode
    /// </summary>
    public class MRKToSGamemode {
        /// <summary>
        /// Gamemode name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gamemode role list
        /// </summary>
        public List<MRKToSRoleID> RoleList { get; private set; }

        public MRKToSGamemode(string name, List<MRKToSRoleID> rlist) {
            Name = name;
            RoleList = rlist;
        }

        public MRKToSGamemode(string name, params MRKToSRoleID[] rlist) {
            Name = name;
            RoleList = rlist.ToList();
        }
    }
}