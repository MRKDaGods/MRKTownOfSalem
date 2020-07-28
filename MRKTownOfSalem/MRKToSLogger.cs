using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK {
    /// <summary>
    /// Generic blind logger
    /// </summary>
    public class MRKToSLogger {
        public delegate void MRKToSLogDelegate(MRKToSLogType ltype, string msg);

        static List<MRKToSLogDelegate> ms_LogDelegates = new List<MRKToSLogDelegate>();

        /// <summary>
        /// Add a new log listener
        /// </summary>
        /// <param name="logDelegate">The listener to add</param>
        public static void RegisterLogger(MRKToSLogDelegate logDelegate) => ms_LogDelegates.Add(logDelegate);

        /// <summary>
        /// Remove an existing log listener
        /// </summary>
        /// <param name="logDelegate">The listener to remove</param>
        public static void UnregisterLogger(MRKToSLogDelegate logDelegate) => ms_LogDelegates.Remove(logDelegate);

        /// <summary>
        /// Broadcast a log, described by given logtype and given message
        /// </summary>
        /// <param name="ltype">Log type</param>
        /// <param name="msg">Message</param>
        public static void Log(MRKToSLogType ltype, string msg) => ms_LogDelegates.ForEach(del => del(ltype, msg));

        /// <summary>
        /// Broadcast a log, described by Info and given message
        /// </summary>
        /// <param name="msg">Message</param>
        public static void LogInfo(string msg) => ms_LogDelegates.ForEach(del => del(MRKToSLogType.Info, msg));

        /// <summary>
        /// Broadcast a log, described by Warning and given message
        /// </summary>
        /// <param name="msg">Message</param>
        public static void LogWarning(string msg) => ms_LogDelegates.ForEach(del => del(MRKToSLogType.Warning, msg));

        /// <summary>
        /// Broadcast a log, described by Error and given message
        /// </summary>
        /// <param name="msg">Message</param>
        public static void LogError(string msg) => ms_LogDelegates.ForEach(del => del(MRKToSLogType.Error, msg));
    }
}