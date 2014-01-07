using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communicator;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Matcher
{
    public class Matcher
    {
        private bool _isRunning = false;
        private MatcherCommander Commander;
        private Processor proc;
        private string status = "";

        private Matcher() { }

        public Matcher(Processor processor, MatcherCommander commander)
        {
            proc = processor;
            Commander = commander;
        }

        public string GetStatus()
        {
            return status;
        }

        public void Start(out Thread thread)
        {
            _isRunning = true;
            thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            status = "stopping";
        }

        private void Run()
        {
            List<Profile> profiles;
            List<Vacancy> vacancies;
            status = "init";
            Debug.WriteLine("Init");
            while (_isRunning)
            {
                //profiles = proc.GetNextProfiles();
                //vacancies = proc.GetNextVacancies
                //Do matching algorithm here
                Debug.WriteLine("Start");
                status = "matching";
                Thread.Sleep(5000);
                //Debug.WriteLine("Done");
            }
            Commander.FinishMatcher(this);
            
        }
    }
}
