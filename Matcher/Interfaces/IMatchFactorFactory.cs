using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public interface IMatchFactorFactory
    {
        MatchFactor CreateMatchFactor(string factor, string text, double strength, int multiplier);
    }
}
