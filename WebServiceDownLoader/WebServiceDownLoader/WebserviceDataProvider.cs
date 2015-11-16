using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace WebServiceDownLoader
{
    public class WebserviceDataProvider : IDisposable
    {
        private readonly ResponseConverter _responseConverter;
        private readonly HttpClient _client;
        public WebserviceDataProvider()
        {
            //Would normally use an IOC container, but this is just a small demo.
            _responseConverter = new ResponseConverter();
            _client = new HttpClient();

        }
        public WebserviceDataProvider(ResponseConverter responseConverter, HttpClient client)
        {
            _responseConverter = responseConverter;
            _client = client;
        }


        public async Task<string> GetXmlFromWebServiceUrl(Uri url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new InvalidDataException($"Unable to retrieve data from URL. HTTP status code: {response.StatusCode}");

            return await _responseConverter.GetXmlFromResponse(response);
        }


        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
