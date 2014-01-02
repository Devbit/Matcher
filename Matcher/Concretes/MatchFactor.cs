using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    class MatchFactor : IMatchFactor
    {
        private string _Factor;
        public string Factor
        {
            get
            {
                return _Factor;
            }
            set
            {
                _Factor = value;
            }
        }
        private int _Strength;
        public int Strength
        {
            get
            {
                return _Strength;
            }
            set
            {
                _Strength = value;
            }
        }
        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
            }
        }
    }
}
