//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■       © Copyright 2014         
//           ■■■■         ■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■       | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■       |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|


using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public interface IMatchFactory
    {
        Match CreateMatch(Profile profile, Vacancy vacancy, List<MatchFactor> factors, double strength);
    }
}
