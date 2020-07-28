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
    public class MRKToSTimer {
        long m_StartTime;
        
        long m_CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        public bool Enabled { get; set; }
        public long Max { get; set; }
        public long Current => m_CurrentTime - m_StartTime;
        public long RelativeCurrent => Max - Current;

        public void Reset() {
            m_StartTime = m_CurrentTime;
        }

        public void Start() {
            Enabled = true;

            Reset();
        }

        public void Stop() {
            Enabled = false;
        }
    }

    public enum MRKTosTimerEventState {
        None,
        Stopped,
        Started,
        Changed,
        Updated
    }

    public class MRKToSTimerEvent {
        public MRKToSTimer Timer { get; private set; }
        public MRKTosTimerEventState State { get; private set; }

        public MRKToSTimerEvent(MRKToSTimer timer, MRKTosTimerEventState state) {
            Timer = timer;
            State = state;
        }
    }
}
