using Communicator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Matcher
{
    public class MatcherCommander
    {
        private ConcurrentDictionary<Matcher, Thread> ThreadStack = new ConcurrentDictionary<Matcher, Thread>();
        Processor Processor;

        public MatcherCommander(string link, bool buffering)
        {
            Processor = new Processor(link, buffering);
        }

        public int GetMatcherCount()
        {
            return ThreadStack.Values.Count;
        }

        public string GetMatcherStatus()
        {
            string result = "";
            int i = 0;
            foreach (Matcher matcher in ThreadStack.Keys)
            {
                result += " [" + i + "] - Matcher@" + matcher.GetHashCode() + " - " + matcher.GetStatus() + Environment.NewLine;
                i++;
            }
            return result;
        }

        public void SpawnMatcher()
        {
            Matcher matcher = new Matcher(Processor, this);
            Thread th;
            matcher.Start(out th);
            ThreadStack.TryAdd(matcher, th);
        }

        public void StopMatcher(int index)
        {
            Matcher matcher = ThreadStack.Keys.ElementAt(index);
            matcher.Stop();
        }

        public void FinishMatcher(Matcher matcher)
        {
            matcher.Stop();
            Thread thread;
            ThreadStack.TryRemove(matcher, out thread);
        }
    }
}
