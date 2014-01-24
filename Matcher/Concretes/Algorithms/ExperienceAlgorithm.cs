//           ■■■■   ■■■■■■              
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
using System.Text;
using System.Threading.Tasks;
using Communicator;
using Matcher.Concretes.Algorithms;
using System.Diagnostics;

namespace Matcher.Algorithms
{

    class ExperienceAlgorithm : IAlgorithm
    {
        
        // Matched Experience and Details
        public MatchFactor CalculateFactor<T>(Profile profile, Vacancy vacancy, int multiplier)
        {
            TextAnalyser analyser = new TextAnalyser(20, "http://api.stackoverflow.com/1.1/tags?pagesize=100&page=");
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            Debug.WriteLine("== START EXPERIENCE ==");

            List<Experience> experiences = profile.experience;
            
            int count = 0; 
            double score = 0;

            for (int expNr = 0; expNr < experiences.Count; expNr++)
            {
                Experience experience = experiences.ElementAt(expNr);
                List<string> matchingWordsExperience = new List<string>();
                 
                if (experience.description != null)
                {
                    matchingWordsExperience.AddRange(analyser.AnalyseText(experience.description)); // Check description
                }
                if (experience.details != null)
                {
                    matchingWordsExperience.AddRange(analyser.AnalyseText(experience.details)); // Also check experience Details
                }
                if (experience.title != null)
                {
                    matchingWordsExperience.AddRange(analyser.AnalyseText(experience.title)); // Also check experience Title
                }
                if (profile.interests != null)
                {                    
                    matchingWordsExperience.AddRange(analyser.AnalyseText(profile.interests)); // And interests
                }
                if (profile.specialties != null)
                {
                    matchingWordsExperience.AddRange(analyser.AnalyseText(profile.specialties)); // And specialties
                }
                if (profile.honors != null)
                {
                    for (int i = 0; i < profile.honors.Count; i++)
                    {
                        string honour = profile.honors.ElementAt(i);
                        matchingWordsExperience.Add(honour);
                    }  
                }

                List<string> matchingWordsVacancy = new List<string>();

                if (vacancy.details.advert_html != null)
                {
                    matchingWordsVacancy.AddRange(analyser.AnalyseText(vacancy.details.advert_html));
                }
                if (vacancy.details.job_type != null)
                {
                    matchingWordsVacancy.AddRange(analyser.AnalyseText(vacancy.details.job_type));
                }
                if (vacancy.title != null)
                {
                    matchingWordsVacancy.AddRange(analyser.AnalyseText(vacancy.title));
                }
                if (vacancy.company != null)
                {
                    matchingWordsVacancy.AddRange(analyser.AnalyseText(vacancy.company));
                }

                
                List<string> matchingWordsCombined = new List<string>();

                if (matchingWordsVacancy != null && matchingWordsExperience != null)
                {
                    matchingWordsCombined = analyser.CompareLists(matchingWordsExperience, matchingWordsVacancy);
                }


                double preScore = 0.0;
                if (matchingWordsVacancy.Count > 0)
                {
                    preScore = preScore + (matchingWordsCombined.Count / matchingWordsVacancy.Count) * 100;
                }

                // Systeem voor gewerkte tijd per experience.
                string start = experience.start;
                string end = experience.end;
                int startInteger = 0;
                int endInteger = 0;

                if (end != null && (end.ToLower().Equals("present") || end.ToLower().Equals("heden")))
                {
                    endInteger = Convert.ToInt32(DateTime.UtcNow.Date.ToString("yyyyMM"));
                }
                else if (end != null)
                {
                    endInteger = TranslateDate(end);
                }

                if (start != null)
                {
                    startInteger = TranslateDate(start);
                }
                int timeWorked = 0;
                if (startInteger != 0 || endInteger != 0)
                {
                    timeWorked = FixDateInt(endInteger - startInteger);
                }

                if (timeWorked > 500)
                {
                    preScore = preScore * 1;
                }
                else
                {
                    double threshold = 0.3;
                    preScore = preScore * (1.0 - threshold + (timeWorked / 500 * threshold));
                }

                score += preScore;
                count++;


            }

            // Gemiddelde voor eerlijke score
            if (count != 0 && score != 0)  
            {

                MatchFactor experienceFactor = matchFactorFactory.CreateMatchFactor("Experience", "", score / Convert.ToDouble(count), multiplier);
                return experienceFactor;
            }
            return null;
        }

        private int TranslateDate(string date)
        {
            date = date.ToLower();

            int year = 0;
            int month = 0;

            if (date.Length >= 7) // YYYY-MM-DD
            {
                month = Convert.ToInt32(date.Substring(5, 2));
                year = Convert.ToInt32(date.Substring(0, 4));
            }
            else if (date != null)// YYYY or error
            {
                month = 1;
                year = Convert.ToInt32(date.Substring(Math.Min(0, 4)));
            }
            else
            {
                int returnValue = Convert.ToInt32(DateTime.UtcNow.Date.ToString("yyyyMM"));
                return returnValue;
            }

            year = year * 100;
            

            return (month + year);
        }

        private int FixDateInt(int dateInt)
        {
            string dateIntString = Convert.ToString(dateInt);
            int partial = Math.Max(0, dateIntString.Length - 2);
            string actualData = dateIntString.Substring(partial);
            int monthFixed = Convert.ToInt32(actualData) % 12;

            if (monthFixed == 0)
            {
                monthFixed = 1;
            }
            int year = Convert.ToInt32(dateIntString.Substring(0, partial)) * 100;
            int result = year + monthFixed;
            return result;
        }
               
    }
}
