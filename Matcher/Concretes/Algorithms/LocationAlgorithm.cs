using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;

namespace Matcher.Algorithms
{
    class LocationAlgorithm : IAlgorithm
    {
        public MatchFactor CalculateFactor<T>(List<T> locations, int multiplier)
        {
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();
            double score = 0;

            if (score != 0)
            {
                MatchFactor experienceFactor = matchFactorFactory.CreateMatchFactor("Experience", "",
                    score, multiplier);
                return experienceFactor;
            }

            return null;
        }
    }
}
