using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public class MRKToSPlayer {
        List<MRKToSRole> m_Roles;
        Dictionary<MRKToSRoleID, MRKToSScroll> m_Scrolls;

        public MRKToSUser User { get; private set; }
        public MRKToSMatch Match { get; private set; }
        public string Name { get; set; }
        public int ID { get; private set; }
        public bool Alive { get; set; }
        public MRKToSRole Role {
            get {
                return m_Roles[m_Roles.Count - 1];
            }

            set {
                m_Roles.Add(value);
            }
        }

        public MRKToSPlayer(MRKToSUser user, MRKToSMatch match, int id) {
            User = user;
            Match = match;
            ID = id;
            m_Roles = new List<MRKToSRole>();
            m_Scrolls = new Dictionary<MRKToSRoleID, MRKToSScroll>(); //TODO import from user
        }

        public void SendMessage(string message) {
            Match.MatchListener.OnPlayerReceiveMessage(message);
        }

        public MRKToSScroll GetScroll(MRKToSRoleID role) {
            if (m_Scrolls.ContainsKey(role))
                return m_Scrolls[role];

            MRKToSScroll scroll = new MRKToSScroll(role, 0);
            m_Scrolls[role] = scroll;

            return scroll;
        }
    }
}
