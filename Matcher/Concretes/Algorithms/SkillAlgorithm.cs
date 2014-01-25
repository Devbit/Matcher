//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■        © Copyright 2014         
//           ■■■■         ■■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■        | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■        |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|


using Communicator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes.Algorithms
{
    class SkillAlgorithm : IAlgorithm
    {
        public MatchFactor CalculateFactor<T>(Profile profile, Vacancy vacancy, int multiplier, MatcherCommander commander)
        {
            Debug.WriteLine("== START SKILLS ==");
            TextAnalyser analyser = new TextAnalyser(commander.GetBag());
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            List<string> skills = profile.skills;

            double strength = 0;
            string analyseThis = "";
            
            for (int i = 0; i < skills.Count; i++)
            {
                analyseThis += " " + skills.ElementAt(i);
            }

            List<string> analysedProfile = analyser.AnalyseText(analyseThis);

            List<string> analysedVacancy = new List<string>();

            if (vacancy.details.advert_html != null)
            {
                analysedVacancy.AddRange(analyser.AnalyseText(vacancy.details.advert_html));
            }
            if (vacancy.details.job_type != null)
            {
                analysedVacancy.AddRange(analyser.AnalyseText(vacancy.details.job_type));
            }
            if (vacancy.title != null)
            {
                analysedVacancy.AddRange(analyser.AnalyseText(vacancy.title));
            }
            if (vacancy.company != null)
            {
                analysedVacancy.AddRange(analyser.AnalyseText(vacancy.company));
            }


            analysedVacancy.AddRange(analyser.AnalyseText(vacancy.details.job_type)); // Also include the Jobtype for better accuracy
            List<string> comparedList = new List<string>();


            if (analysedProfile != null && analysedVacancy != null)
            {
                comparedList = analyser.CompareLists(analysedProfile, analysedVacancy);
            }

            if (comparedList.Count > 0)
            {
                strength = (comparedList.Count / analysedVacancy.Count) * 100;
            }

            MatchFactor skillFactor = matchFactorFactory.CreateMatchFactor("Skills", "", strength, multiplier);
            return skillFactor;
        }
    }
}
