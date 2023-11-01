using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace WaterfoxUpdater
{
    class Program
    {
        static string appData = (Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)).ToString() + "/WaterfoxUpdate/";
        static string fileName = "waterfox.tar.bz2";
        static string opt = (Environment.GetFolderPath(System.Environment.SpecialFolder.System)).ToString() + "/opt/";
        
        static async Task Main(string[] args)
        {
            Console.WriteLine(opt);
            await update();
            clearAppData();
            
        }

        static async Task update()
        {
            var url  = @"https://cdn1.waterfox.net/waterfox/releases/latest/linux";

            if(url != "")
            {
                var downloader = new HttpDownloader(url, appData, fileName);
                if(await downloader.Download())
                {
                    var unzip = new Unzip(appData, opt, fileName);
                    if(unzip.decompresse())
                    {
                        Console.WriteLine("Waterfox updated");
                        return;
                    }
                }
            
            }

            Console.WriteLine("Update Fail");         
        
        }

        static void clearAppData()
        {
            try
            {
                Directory.Delete(appData, true);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
