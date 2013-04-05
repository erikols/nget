using System;
using System.Threading;

namespace nget.core.Utils
{
    public class DelayService : IDelayService
    {
        static void Delay(TimeSpan duration)
        {
            Thread.Sleep(duration);
        }

        public void Delay(int durationInMilliseconds)
        {
            Delay(TimeSpan.FromMilliseconds(durationInMilliseconds));
        }
    }
}