using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace WebServiceDownLoader
{
   public class ResponseConverter
    {
        public async Task<string> GetXmlFromResponse(HttpResponseMessage response)
        {
            String mediaType = response.Content.Headers.ContentType.MediaType;

            if (mediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return await JsonToXML(response.Content.ReadAsStringAsync());
            }
            if (mediaType.Equals("application/xml",StringComparison.OrdinalIgnoreCase))
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new InvalidDataException(
                $"Unable to convert response to XML. response MIME Type: {response.Content.Headers.ContentType.MediaType}");
        }

        public async Task<string> JsonToXML(Task<string> jsonData, string rootNodeName ="root")
        {


            String openingWrapper = "{  '?xml': {    '@version': '1.0',    '@standalone': 'no'  }, ";
            String closingWrapper = "}";


            String wrappedData = $"{openingWrapper}{rootNodeName}: {await jsonData}{closingWrapper}";
            XmlDocument doc = JsonConvert.DeserializeXmlNode(wrappedData);
            return doc.InnerXml;
        }

    }
}
