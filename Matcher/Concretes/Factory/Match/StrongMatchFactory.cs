using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes
{
    public class StrongMatchFactory : MatchFactory
    {
        public override Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors)
        {
            return CreateMatch(profile, vacancy, factors, 75);
        }
    }
}
