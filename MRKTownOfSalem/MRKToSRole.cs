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
    public enum MRKToSRolePriority {
        VeteranKilling = 1,

        BodyguardProtection = 3
    }

    [AttributeUsage(AttributeTargets.Class)]
    sealed class MRKToSRoleReference : Attribute {
        public MRKToSRoleID RoleID {get; private set; }

        public MRKToSRoleReference(MRKToSRoleID roleId) {
            RoleID = roleId;
        }
    }

    public abstract class MRKToSRole {
        List<MRKToSNightDefence> m_NightDefence;

        protected List<MRKToSPlayer> m_TargetsBuffer;
        protected List<MRKToSDynamicButton> m_DynamicButtons;
        protected List<MRKToSPlayer> m_Visitors;

        public abstract MRKToSRoleID RoleID { get; }
        public abstract MRKToSRolePriority RolePriority { get; }

        protected abstract MRKToSDefenceType DefenceType { get; }

        public MRKToSPlayer Player { get; private set; }
        public MRKToSPlayer Target { get; protected set; }

        public MRKToSRole(MRKToSPlayer player) {
            m_NightDefence = new List<MRKToSNightDefence>();

            m_TargetsBuffer = new List<MRKToSPlayer>();
            m_DynamicButtons = new List<MRKToSDynamicButton>();
            m_Visitors = new List<MRKToSPlayer>();

            Player = player;
        }

        protected abstract List<MRKToSPlayer> GetTargets();

        protected MRKToSDynamicButton GetButtonForTarget(int targetId) {
            foreach (MRKToSDynamicButton button in m_DynamicButtons) {
                if (int.Parse(button.Name.Substring(button.Name.LastIndexOf('_') + 1)) == targetId)
                    return button;
            }

            return null;
        }

        protected MRKToSPlayer GetTargetFromButton(MRKToSDynamicButton button) {
            return Player.Match.Players[int.Parse(button.Name.Substring(button.Name.LastIndexOf('_') + 1))];
        }

        protected virtual void OnDynamicButtonStateChanged(MRKToSDynamicButton button, bool newState) {
        }

        public virtual void PerformNightRole() {
            //register visit
            if (Target != null)
                Target.Role.m_Visitors.Add(Player);
        }

        public virtual void OnNightStart() {
            m_Visitors.Clear();

            //add role night defence
            m_NightDefence.Clear();
            m_NightDefence.Add(new MRKToSNightDefence(DefenceType, null));

            //create dynamic buttons
            List<MRKToSPlayer> targets = GetTargets();
            foreach (MRKToSPlayer target in targets)
                m_DynamicButtons.Add(Player.Match.DynamicUI.CreateDynamicButton(Player,
                    $"target_{(int)RoleID}_{target.ID}", OnDynamicButtonStateChanged));
        }

        public virtual void OnNightEnd() {
            //destroy all dynamic buttons
            foreach (MRKToSDynamicButton button in m_DynamicButtons)
                Player.Match.DynamicUI.DestroyDynamicButton(button.ID);

            m_DynamicButtons.Clear();
        }

        public void AddNightDefence(MRKToSDefenceType type, MRKToSPlayer provider) {
            m_NightDefence.Add(new MRKToSNightDefence(type, provider));
        }
    }
}
