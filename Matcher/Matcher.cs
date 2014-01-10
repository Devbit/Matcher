using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communicator;
using System.Collections.Concurrent;
using System.Diagnostics;
using Matcher.Algorithms;
using Matcher.Concretes.Algorithms;

namespace Matcher
{
    public class Matcher
    {
        private bool _isRunning = false;
        private MatcherCommander Commander;
        private Processor proc;
        private string status = "";

        // Set multipliers per category
        private int multiplierExp = 5;
        private int multiplierSkills = 5;
        private double MATCHREQ = 50.0;

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
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();
            MatchFactory matchFactory = new CustomMatchFactory();
            while (_isRunning)
            {
                /*  PRE ALPHA CODE
                 */
                profiles = proc.GetNextProfiles();
                vacancies = proc.GetNextVacancies();
                
                List<MatchFactor> matchFactors = new List<MatchFactor>();
                
                // Matching Algorithm
                for (int profileNr = 0; profileNr < profiles.Count; profileNr++ )
                {
                    Profile profile = profiles.ElementAt(profileNr);
                    Debug.WriteLine(profile);
                    for (int vacancyNr = 0; vacancyNr < vacancies.Count; vacancyNr++ )
                    {
                        Vacancy vacancy = vacancies.ElementAt(vacancyNr);
                        Debug.WriteLine(vacancy);
                        /* ----- COMPARE ALL ELEMENTS ----- */
                        
                        // Experience
                        List<Experience> experience = profile.experience;
                        if (experience != null)
                        {
                            MatchFactor experienceFactor = new ExperienceAlgorithm().CalculateFactor<Experience>(experience, vacancy, multiplierExp);
                            
                            if (experienceFactor != null)
                            {
                                matchFactors.Add(experienceFactor);
                            }
                        }
                        
                        // Skills
                        List<string> skills = profile.skills;
                        if (skills != null)
                        {
                            MatchFactor skillFactor = new SkillAlgorithm().CalculateFactor<string>(skills, vacancy, multiplierSkills);
                            
                            if(skillFactor != null)
                            {
                                matchFactors.Add(skillFactor);
                            }
                        }

                        //


                        // Calculating Strength of a profile with a vacancy
                        double scoreCul = 0.0, multiplierCul = 0.0, strength = 0.0;
                        foreach (MatchFactor fact in matchFactors)
                        {
                            scoreCul += fact.strength * fact.multiplier;
                            multiplierCul += fact.multiplier;
                        }
                        if (scoreCul > 0.0 && multiplierCul > 0.0)
                        {
                            strength = (scoreCul / multiplierCul) * 100;
                        }

                        // When strength is above the Match requirement, a match will be created.
                        if (strength >= MATCHREQ)
                        {
                            Match match = matchFactory.CreateMatch(profile, vacancy, matchFactors, strength);
                            strength = 0;
                            scoreCul = 0;
                            multiplierCul = 0;
                            matchFactors = new List<MatchFactor>();
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
