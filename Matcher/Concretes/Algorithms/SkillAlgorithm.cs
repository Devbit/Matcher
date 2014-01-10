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
            TextAnalyser analyser = new TextAnalyser(5, 10);
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            int score = 0;
            string analyseThis = "";
            
            for (int i = 0; i < skills.Length; i++)
            {
                analyseThis += " " + skills[i];
            }

            List<string> analysedProfile = analyser.AnalyseText(analyseThis);
            List<string> analysedVacancy = analyser.AnalyseText(vacancy.details.ToString());

            List<string> comparedList = analyser.CompareLists(analysedProfile, analysedVacancy);

            if (analysedProfile.Count != 0 && analysedVacancy.Count != 0)
            {
                score += (comparedList.Count / Math.Min(analysedVacancy.Count, analysedProfile.Count)) * 100;
            }

            MatchFactor skillFactor = new MatchFactor();
            return skillFactor;
        }
    }
}
