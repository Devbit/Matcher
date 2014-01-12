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
        List<string> tags;

        public TextAnalyser(int pages, string downloadString)
        {
            tags = fillTags(pages, downloadString);
        }

        public TextAnalyser(string location)
        {
            tags = fillTags(location);
        }

        public List<string> AnalyseText(string x)
        {
            return AnalyseText(x, tags);
        }

        public List<string> AnalyseText(string x, List<string> tags)
        {
            if (tags == null)
            {
                return null;
            }

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


        private List<string> fillTags(int pages, string downloadString)
        {
            // Nieuwe oplossing met API van Stackoverflow
            List<dynamic> jsonResult = new List<dynamic>();

            for (int i = 0; i < pages; i++)
            {
                jsonResult.Add(JsonConvert.DeserializeObject(new WebClient().DownloadString(downloadString + (i+1))));
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

        private List<string> fillTags(string location)
        {
            List<string> list = new List<string>();
                
            int counter = 0;
            string line;

            System.IO.StreamReader file =
               new System.IO.StreamReader(location);
            while ((line = file.ReadLine()) != null)
            {
                list.Add(line.ToLower());
                counter++;
            }

            file.Close();

            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                return list;
            }

            
        }
    }
}
