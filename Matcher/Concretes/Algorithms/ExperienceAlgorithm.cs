using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;
using my.utils;
using Matcher.Concretes.Algorithms;

namespace Matcher.Algorithms
{

    class ExperienceAlgorithm : IExperienceAlgorithm
    {
        
        // Matched Experience and Details
        public MatchFactor CalculateFactor<T>(List<Experience> experiences, Vacancy vacancy, int multiplier)
        {
            TextAnalyser analyser = new TextAnalyser(5, 10);
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            int count = 0, score = 0;
            for (int expNr = 0; expNr < experiences.Count; expNr++)
            {
                Experience experience = experiences.ElementAt(expNr);

                List<string> matchingWordsExperience = analyser.AnalyseText(experience.description);
                matchingWordsExperience.AddRange(analyser.AnalyseText(experience.details));

                List<string> matchingWordsVacancy = analyser.AnalyseText(vacancy.details.ToString());
                List<string> matchingWordsCombined = analyser.CompareLists(
                    matchingWordsExperience,
                    matchingWordsVacancy);

                if (matchingWordsVacancy.Count != 0 && matchingWordsExperience.Count != 0)
                {
                    score += (matchingWordsCombined.Count / Math.Min(matchingWordsVacancy.Count, matchingWordsExperience.Count)) * 100;
                }

                // TODO: Systeem voor gewerkte tijd per experience.
                
                count++;
                

            }

            // Gemiddelde voor eerlijke score
            if (count != 0 && score != 0)
            {
                MatchFactor experienceFactor = matchFactorFactory.CreateMatchFactor("Experience", "", count / score, multiplier);
                return experienceFactor;
            }
            return null;
        }

       
    }
}
