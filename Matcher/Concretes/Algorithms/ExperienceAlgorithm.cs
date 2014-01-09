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
        

        public MatchFactor CalculateFactor<T>(List<Experience> experiences, Vacancy vacancy, int multiplier)
        {
            TextAnalyser analyser = new TextAnalyser();
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            int count = 0, score = 0;
            for (int expNr = 0; expNr < experiences.Count; expNr++)
            {                
                
                List<string> matchingWordsExperience = analyser.AnalyseText(experiences.ElementAt(expNr).description);
                List<string> matchingWordsVacancy = analyser.AnalyseText(vacancy.details.ToString());
                List<string> matchingWordsCombined = analyser.CompareLists(matchingWordsExperience, matchingWordsVacancy);

                score += (matchingWordsCombined.Count / matchingWordsVacancy.Count) * 100;
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
