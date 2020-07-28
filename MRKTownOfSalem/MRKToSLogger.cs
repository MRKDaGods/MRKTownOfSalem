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