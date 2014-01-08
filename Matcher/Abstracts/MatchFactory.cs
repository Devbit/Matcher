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
        public abstract Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors, double strength);
    }
}
