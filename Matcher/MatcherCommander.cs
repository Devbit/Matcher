using Communicator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            StartMatrixFile();
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

        private void StartMatrixFile()
        {
            Process p = new Process();
            try
            {

                string targetDir;

                targetDir = string.Format(@"D:\Development\Visual Studio 2013\Projects\Matcher\Matcher\bin\Debug");
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.WorkingDirectory = targetDir;
                p.StartInfo.FileName = "matrix.bat";
                p.StartInfo.Arguments = string.Format("Devbit Matcher");
                p.StartInfo.CreateNoWindow = false;
                p.Start();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }
    }
}
