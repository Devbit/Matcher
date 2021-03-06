﻿//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■        © Copyright 2014         
//           ■■■■         ■■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■        | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■        |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communicator;
using System.Collections.Concurrent;
using System.Diagnostics;
using Matcher.Algorithms;
using Matcher.Concretes.Algorithms;
using Newtonsoft.Json;

namespace Matcher
{
    public class Matcher
    {
        private bool _isRunning = false;
        private MatcherCommander commander;
        private Processor proc;
        private string status = "";

        // Set multipliers per category
        private int multiplierExp = 5;
        private int multiplierSkills = 5;
        private int multiplierLanguages = 2;

        private const string MatchLink = "matches";

        private Matcher() 
        { 
        
        }

        public Matcher(Processor processor, MatcherCommander commander)
        {
            proc = processor;
            this.commander = commander;
        }

        private void SaveMatch(Match match)
        {
            JsonMatch jsonMatch = new JsonMatch();
            jsonMatch.profile = match.profile._id;
            jsonMatch.vacancy = match.vacancy._id;
            jsonMatch.factors = match.factors;
            jsonMatch.strength = match.strength;
            jsonMatch.date_created = new DateTime().ToString("yyyyMMddHHmmss");
            jsonMatch._id = CreateMD5Hash(jsonMatch.profile + ":" + jsonMatch.vacancy);
            string jsonString = JsonConvert.SerializeObject(jsonMatch);
            proc.InsertDocument(jsonString, MatchLink);
        }

        private string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
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
                    for (int vacancyNr = 0; vacancyNr < vacancies.Count; vacancyNr++)
                    {
                        Vacancy vacancy = vacancies.ElementAt(vacancyNr);
                        /* ----- COMPARE ALL ELEMENTS ----- */

                        // Experience
                        if (profile.experience != null)
                        {
                            MatchFactor experienceFactor = new ExperienceAlgorithm().CalculateFactor<Experience>(profile, vacancy, multiplierExp, commander);

                            if (experienceFactor != null)
                            {
                                matchFactors.Add(experienceFactor);
                            }
                        }

                        // Skills
                        if (profile.skills != null)
                        {
                            MatchFactor skillFactor = new SkillAlgorithm().CalculateFactor<string>(profile, vacancy, multiplierSkills, commander);                           

                            if (skillFactor != null)
                            {
                                matchFactors.Add(skillFactor);
                            }
                        }

                        // Languages
                        if (profile.languages != null)
                        {
                            MatchFactor languageFactor = new LanguageAlgorithm().CalculateFactor<Language>(profile, vacancy, multiplierLanguages, commander);
                            if (languageFactor != null)
                            {
                                matchFactors.Add(languageFactor);
                            }
                        }

                        /* ----- END COMPARING ELEMENTS ----- */

                        //*Debug.WriteLine(matchFactors.Count);
                        // Calculating Strength of a profile with a vacancy
                        double scoreCul = 0.0, multiplierCul = 0.0, strength = 0.0;
                        foreach (MatchFactor fact in matchFactors)
                        {
                            scoreCul += fact.strength * fact.multiplier;
                            multiplierCul += fact.multiplier;
                        }
                        if (scoreCul > 0.0 && multiplierCul > 0.0)
                        {
                            strength = (scoreCul / multiplierCul);
                        }

                        // When strength is above the Match requirement, a match will be created.
                        if (strength > 0)
                        {
                            Match match = matchFactory.CreateMatch(profile, vacancy, matchFactors, strength);
                            SaveMatch(match);
                            Debug.WriteLine("~~~ MATCH CREATED ~~~");
                        }

                        Debug.WriteLine("Profile (" + profile.name.fullname + ") matched Vacancy (" + vacancy._id + ") with " + strength + " stength.");
                        // RESET
                        strength = 0;
                        scoreCul = 0;
                        multiplierCul = 0;
                        matchFactors = new List<MatchFactor>();
                        
                    }
                }
            }

            Debug.WriteLine("Done");
            /*  STOP MATCHING  */

            commander.FinishMatcher(this);
            
        }

    }
}
