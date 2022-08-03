using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WaterfoxUpdater
{
    public class WaterfoxHttpClient
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static string waterfoxUrl = @"https://www.waterfox.net/download/";

        private async Task<string> getHtml()
        {
            var html = "";
            try
            {
                using(httpClient)
                {
                    using(var responce = await httpClient.GetAsync(waterfoxUrl))
                    {
                        using(var content = responce.Content)
                        {
                            html = await content.ReadAsStringAsync();

                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return html;
            
        }

        private async Task<List<string>> getHrefList()
        {
            var html = "";
            var hrefList = new List<string>();
            try
            {
                html = await getHtml();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            if(html == "")
            {
                return new List<string>();
            }

            string pattern = string.Format("href=\"(?<url>.+?)\" ");
            var matches = Regex.Matches(html, pattern);

            foreach(Match match in matches)
            {
                hrefList.Add(match.Groups["url"].Value);
            }

            return hrefList;
        }

        public async Task<string> getUrl()
        {
            var urlList = await getHrefList();
            if(urlList.Count == 0)
            {
                throw new InvalidOperationException("url not exist");
            }

            string shaPattern = string.Format("sha512");
            for(int i = 0; i < urlList.Count; i++)
            {
                if(Regex.IsMatch(urlList[i], shaPattern))
                {
                    urlList.RemoveAt(i);
                }
            }


            string tarPattern = string.Format("tar.bz2");
            foreach(string s in urlList)
            {
                if(Regex.IsMatch(s, tarPattern))
                {
                    return s;
                }
            }

            return "";

        }

    }
}