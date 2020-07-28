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
