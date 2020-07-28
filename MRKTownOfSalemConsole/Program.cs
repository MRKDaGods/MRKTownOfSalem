using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace MRK {
    class Program {
        class MatchListener : MRKToSProxyMatchListener {
            public void OnQueueUserAdded(MRKToSUser user) {
                WriteLine($"[MATCH] User {user.Username} has joined.");
            }

            public void OnQueueUserRemoved(MRKToSUser user) {
                WriteLine($"[MATCH] User {user.Username} has left.");
            }

            public void OnPregameTimerStateChanged(MRKToSTimerEvent evt) {
                switch (evt.State) {

                    case MRKTosTimerEventState.Started:
                        WriteLine($"Pregame timer started, max={evt.Timer.Max}");
                        break;

                    case MRKTosTimerEventState.Stopped:
                        WriteLine($"Pregame timer stopped");
                        break;

                    case MRKTosTimerEventState.Changed:
                        WriteLine($"Pregame timer changed, max={evt.Timer.Max}");
                        break;

                    case MRKTosTimerEventState.Updated:
                        long boundedMils;
                        if (MRKToSUtilities.Bound(evt.Timer.RelativeCurrent, 200L, out boundedMils)) {
                            if (boundedMils % 1000L == 0)
                                WriteLine($"Pregame timer updated, current={evt.Timer.RelativeCurrent / 1000L}");
                        }
                        break;

                }
            }

            public void OnMatchSwitchState(MRKToSMatch.State state) {
                WriteLine($"[MATCH] Switching state [{state}]");

                if (state == MRKToSMatch.State.Names) {
                    ms_Match.AutoAssignPlayerNames();
                    ms_Match.SwitchToState(MRKToSMatch.State.Roles);
                }
            }

            public void OnPlayerReceiveMessage(string message) {

            }

            public void OnMatchRolesAssigned() {
                WriteLine("[MATCH] Roles assigned...");

                foreach (MRKToSPlayer player in ms_Match.Players) {
                    WriteLine($"[MATCH] Player [{player.ID} - {player.Name}] role = {player.Role.RoleID}");
                }
            }
        }

        static MRKToSMatch ms_Match;

        static void Main(string[] args) {
            MRKToSLogger.RegisterLogger((t, m) => WriteLine($"[LOGGER] [{t}] {m}"));

            MRKToSGamemode gamemodeCustom = new MRKToSGamemode("Custom", MRKToSRoleID.Bodyguard, MRKToSRoleID.Bodyguard);
            MRKToSMatchSettings settings = new MRKToSMatchSettings(1, (uint)gamemodeCustom.RoleList.Count, 60, 10, gamemodeCustom);
            MRKToSMatch match = new MRKToSMatch(settings, new MatchListener(), null);
            ms_Match = match;

            match.Start();

            Dictionary<string, MRKToSUser> users = new Dictionary<string, MRKToSUser>();

            while (true) {
                if (match.MatchState == MRKToSMatch.State.Waiting) {
                    Write("Join: ");
                    string user = ReadLine();
                    string _user = user.Replace("_", "");

                    if (!users.ContainsKey(_user)) {
                        users[_user] = new MRKToSUser(_user, 0);
                    }

                    if (user.Contains("_"))
                        match.DequeueUser(users[_user]);
                    else
                        match.QueueUser(users[_user]);
                }
            }
        }
    }
}