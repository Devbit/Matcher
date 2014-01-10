using my.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Matcher.Concretes.Algorithms
{
    class TextAnalyser
    {

        List<string> tags = new List<string>();
        int limit;

        public TextAnalyser(int limit, int pages)
        {
            tags = fillTags(pages);
            this.limit = limit;
        }

        public List<string> AnalyseText(string x)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < tags.Count; i++)
            {

                if (DiffAlgorithm.verifyDiff(x.ToLower(), tags.ElementAt(i).ToLower(), limit))
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


        private List<string> fillTags(int pages)
        {
            // Nieuwe oplossing met API van Stackoverflow
            List<dynamic> jsonResult = new List<dynamic>();

            for (int i = 0; i < pages; i++)
            {
                jsonResult.Add(JsonConvert.DeserializeObject(new WebClient().DownloadString("http://api.stackoverflow.com/1.1/tags?pagesize=100&page=" + (i+1))));
            }

            string filler = "";
            for (int i = 0; i < pages; i++)
            {
                filler += "" + jsonResult.ElementAt(i).tags.name;
                if (i >= pages - 1)
                {
                    filler += ",";
                }
            }

            List<string> result = filler.Split(',').ToList();

            return result;
        }

        private void fillTags() // unused, delete in final.
        {

            // Oude oplossing dmv .txt
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
