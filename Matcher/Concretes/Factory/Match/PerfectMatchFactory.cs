using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public class PerfectMatchFactory : MatchFactory
    {
        public override IMatch CreateMatch(IProfile profile, IVacancy vacancy, List<IMatchFactor> factors)
        {
            return CreateMatch(profile, vacancy, factors, 100);
        }
    }
}
