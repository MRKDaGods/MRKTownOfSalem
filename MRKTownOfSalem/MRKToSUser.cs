using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    /// <summary>
    /// Persistent info about someone
    /// </summary>
    public class MRKToSUser {
        /// <summary>
        /// Username of player
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// Experience of player
        /// </summary>
        public uint Experience { get; private set; }

        public MRKToSUser(string username, uint exp) {
            Username = username;
            Experience = exp;
        }
    }
}