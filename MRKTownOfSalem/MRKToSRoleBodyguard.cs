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
    [MRKToSRoleReference(MRKToSRoleID.Bodyguard)]
    public class MRKToSRoleBodyguard : MRKToSRole {
        int m_VestsLeft;

        public override MRKToSRoleID RoleID => MRKToSRoleID.Bodyguard;
        public override MRKToSRolePriority RolePriority => MRKToSRolePriority.BodyguardProtection;

        protected override MRKToSDefenceType DefenceType => MRKToSDefenceType.None;

        public MRKToSRoleBodyguard(MRKToSPlayer player) : base(player) {
            m_VestsLeft = 1; //1 bulletproof vest
        }

        protected override List<MRKToSPlayer> GetTargets() {
            //all alive players, local if vest > 0
            m_TargetsBuffer.Clear();

            foreach (MRKToSPlayer player in Player.Match.Players) {
                if (!player.Alive)
                    continue;

                if (player == Player && m_VestsLeft == 0)
                    continue;

                m_TargetsBuffer.Add(player);
            }

            return m_TargetsBuffer;
        }

        protected override void OnDynamicButtonStateChanged(MRKToSDynamicButton button, bool newState) {
            MRKToSPlayer newTarget = GetTargetFromButton(button);
            if (newTarget == Target) {
                Target = null;

                Player.SendMessage(MRKToSGameMessages.MRKTOS_GENERIC_TARGETNONE);
                return;
            }

            Player.SendMessage(newTarget == Player ? MRKToSGameMessages.MRKTOS_BODYGUARD_TARGETSELF : 
                string.Format(MRKToSGameMessages.MRKTOS_BODYGUARD_TARGETOTHER, newTarget.Name));

            Target = newTarget;
        }

        public override void PerformNightRole() {
            base.PerformNightRole();

            //self vest
            if (Target == Player && m_VestsLeft > 0) {
                AddNightDefence(MRKToSDefenceType.Basic, Player);
                m_VestsLeft--;
                return;
            }

            Target.Role.AddNightDefence(MRKToSDefenceType.Basic, Player);
        }

        public override void OnNightStart() {
            base.OnNightStart();

            Player.SendMessage(string.Format(MRKToSGameMessages.MRKTOS_BODYGUARD_NIGHTSTART, m_VestsLeft));
        }
    }
}
