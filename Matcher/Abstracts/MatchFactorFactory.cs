using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public abstract class MatchFactorFactory : IMatchFactorFactory
    {
        public abstract MatchFactor CreateMatchFactor(string factor, string text, double strength, int multiplier);
    }
}
