using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public class WeakMatchFactory : MatchFactory
    {
        public override Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors)
        {
            return CreateMatch(profile, vacancy, factors, 25);
        }
    }
}
