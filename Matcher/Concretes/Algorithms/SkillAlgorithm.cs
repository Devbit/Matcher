using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes.Algorithms
{
    class SkillAlgorithm : ISkillAlgorithm
    {
        public MatchFactor CalculateFactor<T>(string[] skills, Vacancy vacancy, int multiplier)
        {
            TextAnalyser analyser = new TextAnalyser(5);
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            int score = 0;
            string analyseThis = "";
            
            for (int i = 0; i < skills.Length; i++)
            {
                analyseThis += " " + skills[i];
            }

            List<string> analysed = analyser.AnalyseText(analyseThis);

            MatchFactor skillFactor = new MatchFactor();
            return skillFactor;
        }
    }
}
