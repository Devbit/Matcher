using Communicator;
using System;
using System.Collections.Generic;
//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■        © Copyright 2014         
//           ■■■■         ■■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■        | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■        |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|


using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes.Algorithms
{
    class LanguageAlgorithm : IAlgorithm
    {
        TextAnalyser textAnalyser = new TextAnalyser("ext_resources/languages.txt");
        

        public MatchFactor CalculateFactor<T>(Profile profile, Vacancy vacancy, int multiplier)
        {
            List<Language> languages = profile.languages;
            
            List<string> vacancyLanguage = textAnalyser.AnalyseText(vacancy.details.advert_html.ToLower());
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            if (vacancyLanguage != null)
            {
                List<string> profileLanguages = new List<string>();

                for (int i = 0; i < languages.Count; i++)
                {
                    profileLanguages.Add(languages.ElementAt(i).name.ToLower());
                }

                List<string> comparedList = new List<string>();

                if (vacancyLanguage != null && profileLanguages != null)
                {
                    comparedList = textAnalyser.CompareLists(vacancyLanguage, profileLanguages);
                }
                
                double strength = 0;

                if (comparedList.Count != 0)
                {
                    strength = (comparedList.Count / vacancyLanguage.Count) * 100;
                }


                MatchFactor languageFactor = matchFactorFactory.CreateMatchFactor("Language", "", strength, multiplier);
                return languageFactor;
            }

            return null;
        }

    }

}
