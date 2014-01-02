using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher
{
    class Match : IMatch
    {
        private IProfile _Profile;
        public IProfile Profile
        {
            get
            {
                return _Profile;
            }
            set
            {
                _Profile = value;
            }
        }
        private IVacancy _Vacancy;
        public IVacancy Vacancy
        {
            get
            {
                return _Vacancy;
            }
            set
            {
                _Vacancy = value;
            }
        }
        private List<IMatchFactor> _Factors;
        public List<IMatchFactor> Factors
        {
            get
            {
                return _Factors;
            }
            set
            {
                _Factors = value;
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
    }
}
