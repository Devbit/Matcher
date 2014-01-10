using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    interface ISkillAlgorithm
    {
        MatchFactor CalculateFactor<T>(string[] skills, Vacancy vacancy, int multiplier);
    }
}
