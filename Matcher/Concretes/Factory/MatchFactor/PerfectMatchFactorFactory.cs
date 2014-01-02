﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    class PerfectMatchFactorFactory : MatchFactorFactory
    {
        public override IMatchFactor CreateMatchFactor(string factor, string text)
        {
            return CreateMatchFactor(factor, text, 100);
        }
    }
}
