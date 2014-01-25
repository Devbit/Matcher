//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■        © Copyright 2014         
//           ■■■■         ■■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■        | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■        |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|


using Communicator;
using Matcher.Concretes.Algorithms;
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
        private ConcurrentBag<string> tags = new ConcurrentBag<string>();
        private TextAnalyser analyser;

        Processor Processor;

        public MatcherCommander(string link, bool buffering)
        {
            Processor = new Processor(link, buffering);
            Processor.SetVacancyAmount(0);

            Console.WriteLine("Loading program...\n");
            analyser = new TextAnalyser(25, "http://api.stackoverflow.com/1.1/tags?pagesize=100&page=");
            Console.WriteLine("List succesfully loaded");
            tags = analyser.FillBag();
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
            //StartMatrixFile();
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

        public ConcurrentBag<string> GetBag()
        {
            return tags;
        }
    }
}
