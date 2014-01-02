using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public interface IMatch
    {
        IProfile Profile { get; set; }
        IVacancy Vacancy { get; set; }
        List<IMatchFactor> Factors { get; set; }
        int Strength { get; set; }

    }
}
