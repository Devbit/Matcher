using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;
using my.utils;

namespace Matcher.Algorithms
{

    class ExperienceAlgorithm : IAlgorithm
    {
        
        public MatchFactor CalculateFactor<T>(List<T> experiences, int multiplier)
        {
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();
            int count = 0, score = 0;
            for (int expNr = 0; expNr < experiences.Count; expNr++)
            {
                T exp = experiences.ElementAt(expNr);
                
                // Get a list of terms related to ICT sector
                // Compare all words in Description of experiences to that list
                // Save all the words which are more than 90% the same
                // Compare all words in Description of vacancy with the list  <
                // Save all the words which are more than 90% the same         \
                // Compare BOTH list of words together                          \
                // Score will be determined by taking (BOTH WORDS TOGETHER / AMOUNT WORDS FOUND IN VACANCY LIST) * 100

                count++;
            }

            // Gemiddelde voor eerlijke score
            if (count != 0 && score != 0)
            {
                MatchFactor experienceFactor = matchFactorFactory.CreateMatchFactor("Experience", "",
                    count / score, multiplier);
                return experienceFactor;
            }
            return null;
        }
    }
}
