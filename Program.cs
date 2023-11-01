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
            Console.WriteLine("Programs folder: " + opt);
            if(await update())
            {
                Console.WriteLine("Successfully Updated!");
            }
            else 
            {
                Console.WriteLine("Update fail");
            }
            clearAppData();
             
        }

        static async Task<bool> update()
        {
            var status = false;
            var url  = @"https://cdn1.waterfox.net/waterfox/releases/latest/linux";

            var downloader = new HttpDownloader(url, appData, fileName);
            try
            {
                if(await downloader.Download())
                {
                    var unzip = new Unzip(appData, opt, fileName);
                    if(unzip.decompresse())
                    {
                        status = true;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return status;       
        
        }

        static void clearAppData()
        {
            if (Directory.Exists(appData))
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
}
