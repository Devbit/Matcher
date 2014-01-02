using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    public interface IMatchFactor
    {
        string Factor { get; set; }
        int Strength { get; set; }
        string Text { get; set; }

    }
}
