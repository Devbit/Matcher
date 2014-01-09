using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;

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
                experiences.ElementAt(expNr);
                // Even bespreken
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
