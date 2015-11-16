using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceDownLoader
{
    class Program
    {
        static int Main(string[] args)
        {
            DownloaderApp app = new DownloaderApp();
            return app.Run(args);
        }
    }
}
