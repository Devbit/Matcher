using Communicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes.Algorithms
{
    class LanguageAlgorithm
    {
        TextAnalyser textAnalyser = new TextAnalyser("ext_resources/languages.txt");
        

        public MatchFactor CalculateFactor<T>(List<Language> languages, Vacancy vacancy, int multiplier)
        {
            List<string> vacancyLanguage = textAnalyser.AnalyseText(vacancy.details.advert_html.ToLower());
            MatchFactorFactory matchFactorFactory = new CustomMatchFactorFactory();

            if (vacancyLanguage != null)
            {
                List<string> profileLanguages = new List<string>();

                for (int i = 0; i < languages.Count; i++)
                {
                    profileLanguages.Add(languages.ElementAt(i).name.ToLower());
                }

                List<string> comparedList = textAnalyser.CompareLists(vacancyLanguage, profileLanguages);
                
                double strength = 0;

                if (comparedList.Count != 0 && comparedList.Count != null)
                {
                    strength = (comparedList.Count / Math.Min(vacancyLanguage.Count, profileLanguages.Count)) * 100;
                }


                MatchFactor languageFactor = matchFactorFactory.CreateMatchFactor("Language", "", strength, multiplier);
                return languageFactor;
            }

            return null;
        }

    }

}
