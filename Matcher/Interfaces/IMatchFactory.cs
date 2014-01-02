using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public interface IMatchFactory
    {
        IMatch CreateMatch(IProfile profile, IVacancy vacancy, List<IMatchFactor> factors);
    }
}
