using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public abstract class MatchFactorFactory : IMatchFactorFactory
    {
        public abstract IMatchFactor CreateMatchFactor(string factor, string text);

        public IMatchFactor CreateMatchFactor(string factor, string text, int strength)
        {
            IMatchFactor matchfactor = new MatchFactor();
            matchfactor.Factor = factor;
            matchfactor.Text = text;
            matchfactor.Strength = strength;
            return matchfactor;
        }
    }
}
