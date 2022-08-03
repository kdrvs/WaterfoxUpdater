using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace WaterfoxUpdater
{
    public class HttpDownloader
    {
        private HttpClient httpClient = new HttpClient();
        private string url;
        private string targetFolder;
        private string fileName;
        
        
        public HttpDownloader(string _url, string _tragetFolder, string _fileName)
        {
            this.url = _url;
            this.targetFolder = _tragetFolder;
            this.fileName = _fileName;
        }

        public async Task<bool> Download()
        {
            var status = false;
            try
            {
                if(!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                using(httpClient)
                {
                    using(var responce = await httpClient.GetAsync(url))
                    {
                        using(Stream stream = await responce.Content.ReadAsStreamAsync())
                        {
                            using(var fileStream = File.Create(targetFolder + fileName))
                            {
                                stream.CopyTo(fileStream);
                                status = true;
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return status;
        }

    }
}