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
    /// <summary>
    /// Match settings
    /// </summary>
    public class MRKToSMatchSettings {
        /// <summary>
        /// Minimum player count to start game
        /// </summary>
        public uint MinimumPlayerCount { get; private set; }
        
        /// <summary>
        /// Maximum player count to start game
        /// </summary>
        public uint MaximumPlayerCount { get; private set; }
        
        /// <summary>
        /// Time in seconds for match to start if the minimum player count is met
        /// </summary>
        public uint MinimumStartTime { get; private set; }
        
        /// <summary>
        /// Time in seconds for match to start if the maximum player count is met
        /// </summary>
        public uint MaximumStartTime { get; private set; }
        
        /// <summary>
        /// Match gamemode
        /// </summary>
        public MRKToSGamemode Gamemode { get; private set; }

        /// <summary>
        /// Probability muliplier for getting a role if a scroll is equipped
        /// </summary>
        public int ScrollMultiplier { get; private set; }

        public MRKToSMatchSettings(uint minCount, uint maxCount, uint minTime, uint maxTime, MRKToSGamemode gamemode, int scrollMultiplier = 5) {
            MinimumPlayerCount = minCount;
            MaximumPlayerCount = maxCount;
            MinimumStartTime = minTime;
            MaximumStartTime = maxTime;
            Gamemode = gamemode;
            ScrollMultiplier = scrollMultiplier;
        }
    }
}
