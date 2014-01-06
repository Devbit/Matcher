﻿using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    class WeakMatchFactorFactory : MatchFactorFactory
    {
        public override MatchFactor CreateMatchFactor(string factor, string text)
        {
            return CreateMatchFactor(factor, text, 25);
        }
    }
}
