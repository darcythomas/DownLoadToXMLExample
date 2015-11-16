using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceDownLoader
{
    public class DownloaderApp
    {
        private WebserviceDataProvider _webserviceDataProvider;

        public DownloaderApp()
        {
            _webserviceDataProvider = new WebserviceDataProvider();
        }

        public DownloaderApp(WebserviceDataProvider webserviceDataProvider)
        {
            _webserviceDataProvider = webserviceDataProvider;
        }
        public int Run(string[] args)
        {
            Uri url;
            FileInfo file;

            if (args.Count() < 2)
            {
                Console.WriteLine($"Usage: WebServiceDownLoader <webservice URL> <file name> {Environment.NewLine} Example: WebServiceDownLoader http://musicbrainz.org/ws/2/artist/5b11f4ce-a62d-471e-81fc-a69a8278c7da?inc=aliases&fmt=json Nirvana.xml");
                return 1;
            }

            string urlString = args[0];
            string filePath = args[1];

            try
            {
                url = new Uri(urlString);
            }
            catch (Exception)
            {
                Console.WriteLine($"The url {urlString} is invalid");
                return 2;
            }

            try
            {
                file = new FileInfo(filePath);
            }
            catch (Exception)
            {
                Console.WriteLine($"The file name {urlString} is invalid");
                return 3;
            }


            Task<string> xmlFromWebService = _webserviceDataProvider.GetXmlFromWebServiceUrl(url);
            File.WriteAllText(file.FullName, xmlFromWebService.Result);

            return 0; //0: All things worked exit code

        }
    }
}
