using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;
using Matcher.Concretes.Algorithms;

namespace Matcher.Algorithms
{

    class ExperienceAlgorithm : IExperienceAlgorithm
    {
        
        // Matched Experience and Details
        public MatchFactor CalculateFactor<T>(List<Experience> experiences, Vacancy vacancy, int multiplier)
        {
            TextAnalyser analyser = new TextAnalyser(10, "http://api.stackoverflow.com/1.1/tags?pagesize=100&page=");
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            int count = 0, score = 0;
            for (int expNr = 0; expNr < experiences.Count; expNr++)
            {
                Experience experience = experiences.ElementAt(expNr);

                List<string> matchingWordsExperience = analyser.AnalyseText(experience.description);
                matchingWordsExperience.AddRange(analyser.AnalyseText(experience.details));

                List<string> matchingWordsVacancy = analyser.AnalyseText(vacancy.details.advert_html);
                List<string> matchingWordsCombined = analyser.CompareLists(
                    matchingWordsExperience,
                    matchingWordsVacancy);

                if (matchingWordsCombined.Count != 0 && matchingWordsCombined.Count != null)
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
