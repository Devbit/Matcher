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
        public abstract MatchFactor CreateMatchFactor(string factor, string text);

        public MatchFactor CreateMatchFactor(string factor, string text, int strength)
        {
            MatchFactor matchfactor = new MatchFactor();
            matchfactor.factor = factor;
            matchfactor.text = text;
            matchfactor.strength = strength;
            return matchfactor;
        }
    }
}
