using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator;

namespace Matcher
{
    class CustomMatchFactorFactory : MatchFactorFactory
    {
        public override MatchFactor CreateMatchFactor(string factor, string text, double strength, int multiplier)
        {
            MatchFactor matchfactor = new MatchFactor();
            matchfactor.factor = factor;
            matchfactor.text = text;
            matchfactor.strength = strength;
            matchfactor.multiplier = multiplier;
            return matchfactor;
        }
    }
}
