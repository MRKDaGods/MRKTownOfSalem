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
