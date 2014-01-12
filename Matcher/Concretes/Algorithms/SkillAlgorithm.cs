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
        public MatchFactor CalculateFactor<T>(List<string> skills, Vacancy vacancy, int multiplier)
        {
            TextAnalyser analyser = new TextAnalyser(10, "http://api.stackoverflow.com/1.1/tags?pagesize=100&page=");
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            double strength = 0;
            string analyseThis = "";
            
            for (int i = 0; i < skills.Count; i++)
            {
                analyseThis += " " + skills.ElementAt(i);
            }

            List<string> analysedProfile = analyser.AnalyseText(analyseThis);
            List<string> analysedVacancy = analyser.AnalyseText(vacancy.details.advert_html);

            List<string> comparedList = analyser.CompareLists(analysedProfile, analysedVacancy);

            if (comparedList.Count != 0 && comparedList.Count != null)
            {
                strength = (comparedList.Count / Math.Min(analysedVacancy.Count, analysedProfile.Count)) * 100;
            }

            MatchFactor skillFactor = matchFactorFactory.CreateMatchFactor("Skills", "", strength, multiplier);
            return skillFactor;
        }
    }
}
