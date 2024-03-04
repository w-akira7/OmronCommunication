using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.Thread
{
    public sealed class HybirdLock
    {
        private int m_waiters = 0;
        AutoResetEvent m_waitLock = new AutoResetEvent(false);

        public void Enter()
        {
            //如果只有一个线程，直接返回
            if (Interlocked.Increment(ref m_waiters) == 1)
                return;

            //1个以上的线程在这里被阻塞
            m_waitLock.WaitOne();
        }

        public void Leave()
        {
            //如果只有一个线程，直接返回
            if (Interlocked.Decrement(ref m_waiters) == 0)
                return;

            //如果有多个线程等待，就唤醒一个
            m_waitLock.Set();
        }
    }
}
