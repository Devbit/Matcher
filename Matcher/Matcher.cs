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

        // Set multipliers per category
        int multiplierExp = 5;

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
                    
            // Variables for Matching   
            int    multiplierCul = 0,       // All multipliers culumative
                   scoreCul = 0;            // All score culumative
            double MATCHREQ = 50.00;        // The minimum score to be considered a match
            //   

            status = "init";
            Debug.WriteLine("Init");
            while (_isRunning)
            {
                /*  PRE ALPHA CODE
                 *
                profiles = proc.GetNextProfiles();
                vacancies = proc.GetNextVacancies();
                
                // Matching Algorithm
                for (int profileNr = 0; profileNr < profiles.Count(); profileNr++ )
                {
                    Profile profile = profiles.ElementAt(profileNr);
                    
                    for (int vacancyNr = 0; vacancyNr < vacancies.Count(); vacancyNr++ )
                    {
                        Vacancy vacancy = vacancies.ElementAt(vacancyNr);


                        /* ----- COMPARE ALL ELEMENTS ----- *
                        
                        /* EXPERIENCE SCORE *
                        int totalExp = 0, scoreExp = 0;
                        List<Experience> experience = profile.experience;

                        for ( int expNr = 0; expNr < experience.Count(); expNr++ )
                        {
                            experience.ElementAt(expNr);
                            // Even bespreken
                            totalExp++;
                        }
                        // Gemiddelde voor eerlijke score
                        if (totalExp != 0 && scoreExp != 0)
                        {
                            scoreCul += ((scoreExp / totalExp) * 100) * multiplierExp;
                            multiplierCul += multiplierExp;
                        }

                        /* OTHER CATEGORY */
                        



                        /*
                         * Hier moeten we nog even een oplossing voor zoeken aangezien het algoritme wat ik heb gemaakt
                         * is niet toepasbaar. Nieuwe berekening van score is vereist
                         *


                        
                        // Strength is the percentage a profile matches with a vacancy.
                        double strength = (scoreCul / multiplierCul) * 100;

                        // When strength is above the Match requirement, a match will be created.
                        if (strength >= MATCHREQ)
                        {
                            new CustomMatchFactory(profile, vacancy, /* Factors *, strength);
                            strength = 0;
                            scoreCul = 0;
                            multiplierCul = 0;
                        }
                    }
                }

                    

                //*/
                
                Debug.WriteLine("Start");
                status = "matching";
                Thread.Sleep(5000);
                //Debug.WriteLine("Done");
            }
            Commander.FinishMatcher(this);
            
        }
    }
}
