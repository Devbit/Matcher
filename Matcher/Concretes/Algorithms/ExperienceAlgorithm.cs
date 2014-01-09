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
                
                Experience experience = experiences.ElementAt(expNr);

                // Get a list of terms related to ICT sector vv
                // Compare all words in Description of experiences to that list
                // Save all the words which are more than 90% the same
                // Compare all words in Description of vacancy with the list  <
                // Save all the words which are more than 90% the same         \
                // Compare BOTH list of words together                          \
                // Score will be determined by taking (BOTH WORDS TOGETHER / AMOUNT WORDS FOUND IN VACANCY LIST) * 100



                List<string> matchingWordsExperience = analyser.AnalyseText(experience.description);
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
