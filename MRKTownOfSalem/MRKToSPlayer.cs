/*
 * Copyright (c) 2020, Mohamed Ammar <mamar452@gmail.com>
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
