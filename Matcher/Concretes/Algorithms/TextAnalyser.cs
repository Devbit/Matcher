using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes.Algorithms
{
    class TextAnalyser
    {

        List<string> tags = new List<string>();

        public TextAnalyser()
        {
            fillTags();
        }

        public List<string> AnalyseText(string x)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < tags.Count; i++)
            {

                if (x.ToLower().Contains(tags.ElementAt(i).ToLower()))
                {
                    list.Add(tags.ElementAt(i).ToLower());
                }

            }

            return list;
        }

        public List<string> CompareLists(List<string> list1, List<string> list2)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < list1.Count; i++)
                {
                    for (int j = 0; j < list2.Count; j++)
                    {
                        if (list1.ElementAt(i).Equals(list2.ElementAt(j)))
                        {
                            list.Add(list1.ElementAt(i));
                        }
                    }
                }

            return list;
        }


        private void fillTags()
        {
            int counter = 0;
            string line;

            System.IO.StreamReader file =
               new System.IO.StreamReader("tags.txt");
            while ((line = file.ReadLine()) != null)
            {
                tags.Add(line.ToLower());
                counter++;
            }

            file.Close();
        }


    }
}
