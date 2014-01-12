using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    interface ILanguageAlgorithm
    {
        MatchFactor CalculateFactor<T>(List<Language> list, Vacancy vacancy, int multiplier);
    }
}
