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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRK {
    /// <summary>
    /// Generic TownOfSalem match
    /// </summary>
    public class MRKToSMatch {
        public enum State {
            None,
            Waiting,
            Names,
            Roles
        }

        struct RoleAssignmentContext {
            public int Lots;
            public MRKToSScroll Scroll;
        }

        static readonly Dictionary<MRKToSRoleID, Type> ms_RoleAssignmentMap;

        List<MRKToSUser> m_QueuedUsers;
        Thread m_PregameTimerThread;

        /// <summary>
        /// Match settings
        /// </summary>
        public MRKToSMatchSettings Settings { get; private set; }
        public MRKToSProxyMatchListener MatchListener { get; private set; }
        public MRKToSProxyDynamicUIListener UIListener { get; private set; }
        public MRKToSDynamicUI DynamicUI { get; private set; }
        public MRKToSTimer PregameTimer { get; private set; }
        public List<MRKToSPlayer> Players { get; private set; }
        public State MatchState { get; private set; }

        static MRKToSMatch() {
            ms_RoleAssignmentMap = new Dictionary<MRKToSRoleID, Type>();

            foreach (Type type in Assembly.GetExecutingAssembly().ManifestModule.GetTypes()) {
                MRKToSRoleReference roleRef = type.GetCustomAttribute<MRKToSRoleReference>();
                if (roleRef != null)
                    ms_RoleAssignmentMap[roleRef.RoleID] = type;
            }
        }

        public MRKToSMatch(MRKToSMatchSettings settings, MRKToSProxyMatchListener matchListener, MRKToSProxyDynamicUIListener uiListener) {
            Settings = settings;
            m_QueuedUsers = new List<MRKToSUser>();
            MatchListener = matchListener;
            UIListener = uiListener;
            DynamicUI = new MRKToSDynamicUI();
            PregameTimer = new MRKToSTimer();
        }

        public void Start() {
            MatchState = State.Waiting;

            MRKToSLogger.LogInfo("MRKTownOfSalem started... v1\nBy MRKDaGods\n\n");
        }

        /// <summary>
        /// Add a user to the queue
        /// </summary>
        /// <param name="user"></param>
        public bool QueueUser(MRKToSUser user) {
            if (MatchState != State.Waiting) {
                MRKToSLogger.LogError($"Can not queue {user.Username} to the match, match has already started or is still idle");
                return false;
            }

            if (FindQueuedUser(user.Username) != null) {
                MRKToSLogger.LogWarning($"User {user.Username} is already queued");
                return false;
            }

            if (m_QueuedUsers.Count >= Settings.MaximumPlayerCount) {
                MRKToSLogger.LogWarning($"Can not que {user.Username} to the match, match has maximum player count");
                return false;
            }

            m_QueuedUsers.Add(user);

            MRKToSLogger.LogInfo($"User {user.Username} has been added to the queue");
            LogQueueCount();

            MatchListener.OnQueueUserAdded(user);

            UpdatePregame();
            return true;
        }

        /// <summary>
        /// Remove a user from the queue
        /// </summary>
        /// <param name="user"></param>
        public bool DequeueUser(MRKToSUser user) {
            if (MatchState != State.Waiting) {
                MRKToSLogger.LogError($"Can not dequeue {user.Username} to the match, match has already started or is still idle");
                return false;
            }

            if (FindQueuedUser(user.Username) == null) {
                MRKToSLogger.LogWarning($"User {user.Username} is not queued");
                return false;
            }

            m_QueuedUsers.Remove(user);

            MRKToSLogger.LogInfo($"User {user.Username} has been removed from the queue");
            LogQueueCount();

            MatchListener.OnQueueUserRemoved(user);

            UpdatePregame();
            return true;
        }

        /// <summary>
        /// Searches for an already queued user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns></returns>
        MRKToSUser FindQueuedUser(string username) {
            return m_QueuedUsers.Find(user => user.Username == username);
        }

        /// <summary>
        /// Logs the queue count
        /// </summary>
        void LogQueueCount() {
            MRKToSLogger.LogInfo($"Queue[{m_QueuedUsers.Count}/{Settings.MaximumPlayerCount}]");
        }

        /// <summary>
        /// Updates the pregame counter
        /// </summary>
        void UpdatePregame() {
            if (Settings.MinimumPlayerCount > m_QueuedUsers.Count) {
                if (PregameTimer.Enabled) {
                    //disable timer
                    PregameTimer.Stop();

                    MatchListener.OnPregameTimerStateChanged(new MRKToSTimerEvent(PregameTimer, MRKTosTimerEventState.Stopped));

                    MRKToSLogger.LogInfo("Pregame timer state: start -> stop due to 0");
                }
            }
            else if (m_QueuedUsers.Count >= Settings.MinimumPlayerCount) {
                if (!PregameTimer.Enabled) {
                    //start timer
                    PregameTimer.Max = Settings.MinimumStartTime * 1000;
                    PregameTimer.Start();

                    MatchListener.OnPregameTimerStateChanged(new MRKToSTimerEvent(PregameTimer, MRKTosTimerEventState.Started));

                    MRKToSLogger.LogInfo("Pregame timer state: stop -> start due to 1");
                }

                if (m_QueuedUsers.Count == Settings.MaximumPlayerCount) {
                    if (PregameTimer.Max == Settings.MinimumStartTime * 1000) {
                        PregameTimer.Max = Settings.MaximumStartTime * 1000;
                        PregameTimer.Reset();

                        MatchListener.OnPregameTimerStateChanged(new MRKToSTimerEvent(PregameTimer, MRKTosTimerEventState.Changed));

                        MRKToSLogger.LogInfo("Pregame timer state: reset due to 2");
                    }
                }
                else {
                    if (PregameTimer.Max == Settings.MaximumStartTime * 1000) {
                        PregameTimer.Max = Settings.MinimumStartTime * 1000;
                        PregameTimer.Reset();

                        MatchListener.OnPregameTimerStateChanged(new MRKToSTimerEvent(PregameTimer, MRKTosTimerEventState.Changed));

                        MRKToSLogger.LogInfo("Pregame timer state: reset due to 3");
                    }
                }
            }

            if (m_PregameTimerThread == null)
                (m_PregameTimerThread = new Thread(PregameTimerThread)).Start();
        }

        void PregameTimerThread() {
            MRKToSLogger.LogInfo("Pregame timer thread started");

            while (true) {
                if (PregameTimer.Enabled) {
                    MatchListener.OnPregameTimerStateChanged(new MRKToSTimerEvent(PregameTimer, MRKTosTimerEventState.Updated));

                    if (PregameTimer.RelativeCurrent <= 0) {
                        MRKToSLogger.LogInfo("Pregame timer ended");

                        Players = new List<MRKToSPlayer>();
                        foreach (MRKToSUser user in m_QueuedUsers)
                            Players.Add(new MRKToSPlayer(user, this, Players.Count + 1));

                        SwitchToState(State.Names);
                        break;
                    }
                }

                Thread.Sleep(200); // i like to sleep for 200 mils
            }
        }

        /// <summary>
        /// Automatically assigns random names for players
        /// </summary>
        public void AutoAssignPlayerNames() {
            string[] playerNames = MRKToSNameGen.GenerateNames(Players.Count, MRKToSNameGenType.Stock);

            int idx = 0;
            foreach (MRKToSPlayer player in Players)
                player.Name = playerNames[idx++];
        }


        /// <summary>
        /// Assings roles for players
        /// </summary>
        void AssignPlayerRoles() {
            Random rnd = new Random();

            List<MRKToSPlayer> unassigned = new List<MRKToSPlayer>();
            foreach (MRKToSPlayer player in Players)
                unassigned.Add(player);

            foreach (MRKToSRoleID role in Settings.Gamemode.RoleList) {
                Dictionary<MRKToSPlayer, RoleAssignmentContext> ctx = new Dictionary<MRKToSPlayer, RoleAssignmentContext>();
                int totalLots = 1;
                int[] playerMap = new int[unassigned.Count];

                int idx = 0;
                foreach (MRKToSPlayer player in unassigned) {
                    MRKToSScroll scroll = player.GetScroll(role);
                    int playerLots = 10 * (scroll.Count > 0 ? Settings.ScrollMultiplier : 1);

                    playerMap[idx++] = totalLots;

                    totalLots += playerLots;

                    ctx[player] = new RoleAssignmentContext {
                        Lots = playerLots,
                        Scroll = scroll
                    };
                }

                int num = rnd.Next(totalLots) + 1;
                int playerIdxForRole = -1;
                for (int i = unassigned.Count - 1; i > -1; i--) {
                    if (num > playerMap[i]) {
                        playerIdxForRole = i;
                        break;
                    }
                }

                MRKToSPlayer __player = unassigned[playerIdxForRole];
                if (ctx[__player].Scroll.Count > 0)
                    ctx[__player].Scroll.Count--;

                __player.Role = (MRKToSRole)Activator.CreateInstance(ms_RoleAssignmentMap[role], __player);
                unassigned.Remove(__player);
            }

            MatchListener.OnMatchRolesAssigned();
        }

        public void SwitchToState(State newState) {
            switch (newState) {
                case State.Names:
                    if (MatchState >= State.Names)
                        return;

                    break;

                case State.Roles:
                    if (MatchState != State.Names)
                        return;

                    AssignPlayerRoles();
                    break;
            }

            MatchState = newState;
            MatchListener.OnMatchSwitchState(MatchState);
        }
    }
}