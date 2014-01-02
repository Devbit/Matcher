using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public abstract class MatchFactory : IMatchFactory
    {
        public abstract IMatch CreateMatch(IProfile profile, IVacancy vacancy, List<IMatchFactor> factors);

        public IMatch CreateMatch(IProfile profile, IVacancy vacancy, List<IMatchFactor> factors, int strength)
        {
            IMatch match = new Match();
            match.Profile = profile;
            match.Vacancy = vacancy;
            match.Factors = factors;
            match.Strength = strength;
            return match;
        }
    }
}
