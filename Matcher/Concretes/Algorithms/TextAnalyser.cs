//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■        © Copyright 2014         
//           ■■■■         ■■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■        | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■        |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|


using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System.Diagnostics;

namespace Matcher.Concretes.Algorithms
{
    class TextAnalyser
    {
        static List<string> tags;

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
            if (tags == null || x == null)
            {
                return null;
            }

            List<string> list = new List<string>();
            x = x.ToLower();
            string tag, pattern;
            Match m;

            for (int i = 0; i < tags.Count; i++)
            {
                tag = tags.ElementAt(i).Replace("+", "\\W");
                pattern = @"(\s|\W)(?:" + tag + @")(\W|\s|$)";
                m = Regex.Match(x, pattern);
                if (m.Success)
                {
                    list.Add(tags.ElementAt(i));
                }
            }
            list = list.Distinct().ToList();
            list.Remove("class");
            list.Remove("div");
            

            return list;
        }

        public List<string> CompareLists(List<string> list1, List<string> list2)
        {
            List<string> list = new List<string>();

            Debug.WriteLineIf(list1 == null, "BUG: LIST 1 IS NULL! THIS SHOULDNT HAPPEN");
            Debug.WriteLineIf(list2 == null, "BUG: LIST 2 IS NULL! THIS SHOULDNT HAPPEN");
            if (list1 == null || list2 == null)
            {
                return list;
            }

            for (int i = 0; i < list1.Count; i++)
                {
                    for (int j = 0; j < list2.Count; j++)
                    {
                        if (list1.ElementAt(i) != null || list2.ElementAt(j) != null)
                        {
                            if (list1.ElementAt(i).Equals(list2.ElementAt(j)))
                            {
                                list.Add(list1.ElementAt(i));
                            }
                        }
                    }
                }

            return list;
        }


        private List<string> fillTags(int pages, string downloadString)
        {
            if (tags == null)
            {
                // Nieuwe oplossing met API van Stackoverflow
                List<dynamic> jsonResult = new List<dynamic>();
                var client = new RestClient("http://api.stackoverflow.com");
                client.AddHandler("application/json", new DynamicJsonDeserializer());


                for (int i = 0; i < pages; i++)
                {
                    var request = new RestRequest("1.1/tags");
                    request.AddParameter("pagesize", "100");
                    string apiString = downloadString + (i + 1);
                    request.AddParameter("page", i + 1);
                    IRestResponse<JObject> response = client.Execute<JObject>(request);
                    jsonResult.Add(response.Data);
                }

                string filler = "";
                for (int i = 0; i < pages; i++)
                {
                    var enumerator = ((JArray) jsonResult.ElementAt(i)["tags"]).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var j = enumerator.Current;
                        string tag = (j["name"].ToString()).ToLower();
                        if (tag.Length < 2)
                        {
                            tag = " " + tag + " ";
                        }
                        filler += tag + ",";
                    }
                    if (i >= pages - 1)
                    {
                        filler += " ";
                    }
                }

                List<string> result = filler.Split(',').ToList();
                result.RemoveAt(result.Count - 1);

                return result;
            }
            else
            {
                return tags;
            }
            
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

    internal class DynamicJsonDeserializer : IDeserializer
    {
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<dynamic>(response.Content);
        }
    }
}
