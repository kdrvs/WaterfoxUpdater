using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace WaterfoxUpdater
{
    public class Unzip
    {
        public string appData;
        public string zipName;
        public string target;

        public Unzip(string _appData, string _target, string _zipName)
        {
            this.appData = _appData;
            this.target = _target;
            this.zipName = _zipName;
        }

        public bool decompresse()
        {
            var status = false;
            try
            {
                using(Process process = new Process())
                {
                    process.StartInfo.FileName = "tar";
                    process.StartInfo.Arguments = $"-xf {appData}{zipName} -C {target}";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    status = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Unzip Fail: " + e.Message);
                status = false;
            }
            return status;
        }

    }
}