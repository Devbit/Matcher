//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■       © Copyright 2014         
//           ■■■■         ■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■       | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■       |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|

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
        private int multiplierLanguages = 2;
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

            /*  START MATCHING  */
            Debug.WriteLine("Start");
            status = "matching";


            while (_isRunning)
            {
                profiles = proc.GetNextProfiles();
                vacancies = proc.GetNextVacancies();
                
                List<MatchFactor> matchFactors = new List<MatchFactor>();
                
                // Matching Algorithm
                for (int profileNr = 0; profileNr < profiles.Count; profileNr++)
                {
                    Profile profile = profiles.ElementAt(profileNr);
                    Debug.WriteLine(profile);
                    for (int vacancyNr = 0; vacancyNr < vacancies.Count; vacancyNr++)
                    {
                        Vacancy vacancy = vacancies.ElementAt(vacancyNr);
                        Debug.WriteLine(vacancy);
                        /* ----- COMPARE ALL ELEMENTS ----- */

                        // Experience
                        if (profile.experience != null)
                        {
                            MatchFactor experienceFactor = new ExperienceAlgorithm().CalculateFactor<Experience>(profile, vacancy, multiplierExp);

                            if (experienceFactor != null)
                            {
                                matchFactors.Add(experienceFactor);
                            }
                        }

                        // Skills
                        if (profile.skills != null)
                        {
                            MatchFactor skillFactor = new SkillAlgorithm().CalculateFactor<string>(profile, vacancy, multiplierSkills);

                            if (skillFactor != null)
                            {
                                matchFactors.Add(skillFactor);
                            }
                        }

                        // Languages
                        if (profile.languages != null)
                        {
                            MatchFactor languageFactor = new LanguageAlgorithm().CalculateFactor<Language>(profile, vacancy, multiplierLanguages);
                            if (languageFactor != null)
                            {
                                matchFactors.Add(languageFactor);
                            }
                        }

                        /* ----- END COMPARING ELEMENTS ----- */


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
            }

            Debug.WriteLine("Done");
            /*  STOP MATCHING  */

            Commander.FinishMatcher(this);
            
        }

    }
}
