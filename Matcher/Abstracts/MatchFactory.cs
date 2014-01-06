using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;

namespace Matcher
{
    public abstract class MatchFactory : IMatchFactory
    {
        public abstract Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors);

        public Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors, int strength)
        {
            Match match = new Match();
            match.profile = profile;
            match.vacancy = vacancy;
            match.factors = factors;
            match.strength = strength;
            return match;
        }
    }
}
