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
