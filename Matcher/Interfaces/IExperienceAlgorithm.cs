using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Communicator;

namespace Matcher
{
    interface IExperienceAlgorithm
    {
        MatchFactor CalculateFactor<T>(List<Experience> list, Vacancy vacancy, int multiplier);
    }
}
