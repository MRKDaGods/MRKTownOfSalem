using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    public interface MRKToSProxyMatchListener {
        /// <summary>
        /// Raised when a user is queued
        /// </summary>
        /// <param name="user"></param>
        void OnQueueUserAdded(MRKToSUser user);

        /// <summary>
        /// Raised when a user is dequeued
        /// </summary>
        /// <param name="user"></param>
        void OnQueueUserRemoved(MRKToSUser user);

        void OnPregameTimerStateChanged(MRKToSTimerEvent evt);

        void OnMatchSwitchState(MRKToSMatch.State state);

        void OnPlayerReceiveMessage(string message);

        void OnMatchRolesAssigned();
    }
}