using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes
{
    public class StrongMatchFactory : MatchFactory
    {
        public override IMatch CreateMatch(IProfile profile, IVacancy vacancy, List<IMatchFactor> factors)
        {
            return CreateMatch(profile, vacancy, factors, 75);
        }
    }
}
