using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public interface IMatchFactory
    {
        Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors, double strength);
    }
}
