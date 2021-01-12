using System;
using System.Threading;

namespace Force.Common.LightMessager.Common
{
    internal static class RandomUtil
    {
        private static int _seedCounter;
        private static readonly ThreadLocal<Random> ThreadRandom;

        static RandomUtil()
        {
            ThreadRandom = new ThreadLocal<Random>(
                () => new Random((int)DateTime.UtcNow.Ticks + Interlocked.Increment(ref _seedCounter)));
        }

        public static Random Random
        {
            get { return ThreadRandom.Value; }
        }
    }
}
