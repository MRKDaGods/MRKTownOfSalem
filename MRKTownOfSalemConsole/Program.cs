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