using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    class CustomMatchFactory : MatchFactory
    {
        public override Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors, double strength)
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
