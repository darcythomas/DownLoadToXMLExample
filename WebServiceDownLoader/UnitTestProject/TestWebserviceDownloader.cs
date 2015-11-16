using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WebServiceDownLoader;

namespace UnitTestProject
{
    [TestFixture]
    public class TestWebserviceDownloader
    {
        [Test]
        public void Can_convert_JSON_to_XML()
        {

            Assert.DoesNotThrow(() =>
            {
                //Arrange
                ResponseConverter responseConverter = new ResponseConverter();

                //Data retrieved from: http://musicbrainz.org/ws/2/artist/5b11f4ce-a62d-471e-81fc-a69a8278c7da?inc=aliases&fmt=json
                String jsonData =
                    "{\"ipis\":[],\"aliases\":[{\"locale\":null,\"name\":\"Nirvana US\",\"sort-name\":\"Nirvana US\",\"primary\":null,\"type\":null}],\"id\":\"5b11f4ce-a62d-471e-81fc-a69a8278c7da\",\"end_area\":null,\"sort-name\":\"Nirvana\",\"disambiguation\":\"90s US grunge band\",\"country\":\"US\",\"life-span\":{\"begin\":\"1988-01\",\"ended\":true,\"end\":\"1994-04-05\"},\"begin_area\":{\"iso_3166_3_codes\":[],\"id\":\"a640b45c-c173-49b1-8030-973603e895b5\",\"iso_3166_1_codes\":[],\"disambiguation\":\"\",\"sort-name\":\"Aberdeen\",\"name\":\"Aberdeen\",\"iso_3166_2_codes\":[]},\"name\":\"Nirvana\",\"area\":{\"disambiguation\":\"\",\"name\":\"United States\",\"sort-name\":\"United States\",\"iso_3166_2_codes\":[],\"iso_3166_3_codes\":[],\"iso_3166_1_codes\":[\"US\"],\"id\":\"489ce91b-6658-3307-9877-795b68554c98\"},\"gender\":null,\"type\":\"Group\"}";

                Task<string> jsonDataAsTask = Task.Run(() => jsonData);

                //Act
                String actual = responseConverter.JsonToXML(jsonDataAsTask).Result;

                //Assert

                //No exceptions thrown
            });
        }

        [Test]
        public void Can_readXML_fromApplication_JSON_Response_MIMETypes()
        {

            Assert.DoesNotThrow(() =>
            {
                //Arrange

                String mimeType = "application/json";
                //Data retrieved from: http://musicbrainz.org/ws/2/artist/5b11f4ce-a62d-471e-81fc-a69a8278c7da?inc=aliases&fmt=json
                String jsonData =
                    "{\"ipis\":[],\"aliases\":[{\"locale\":null,\"name\":\"Nirvana US\",\"sort-name\":\"Nirvana US\",\"primary\":null,\"type\":null}],\"id\":\"5b11f4ce-a62d-471e-81fc-a69a8278c7da\",\"end_area\":null,\"sort-name\":\"Nirvana\",\"disambiguation\":\"90s US grunge band\",\"country\":\"US\",\"life-span\":{\"begin\":\"1988-01\",\"ended\":true,\"end\":\"1994-04-05\"},\"begin_area\":{\"iso_3166_3_codes\":[],\"id\":\"a640b45c-c173-49b1-8030-973603e895b5\",\"iso_3166_1_codes\":[],\"disambiguation\":\"\",\"sort-name\":\"Aberdeen\",\"name\":\"Aberdeen\",\"iso_3166_2_codes\":[]},\"name\":\"Nirvana\",\"area\":{\"disambiguation\":\"\",\"name\":\"United States\",\"sort-name\":\"United States\",\"iso_3166_2_codes\":[],\"iso_3166_3_codes\":[],\"iso_3166_1_codes\":[\"US\"],\"id\":\"489ce91b-6658-3307-9877-795b68554c98\"},\"gender\":null,\"type\":\"Group\"}";

                ResponseConverter responseConverter = new ResponseConverter();

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(jsonData, Encoding.Unicode, mimeType);

                //Act
                var actual = responseConverter.GetXmlFromResponse(response).Result;

                //Assert

                //No exceptions thrown
            });
        }
        [Test]
        public void Can_readXML_fromApplication_XML_Response_MIMETypes()
        {

            Assert.DoesNotThrow(() =>
            {
                //Arrange

                String mimeType = "application/xml";
                //Data retrieved from: http://musicbrainz.org/ws/2/artist/5b11f4ce-a62d-471e-81fc-a69a8278c7da?inc=aliases&fmt=json
                String xmlData = "<?xml version=\"1.0\" standalone=\"no\"?><root><area><iso_3166_1_codes>US</iso_3166_1_codes><name>United States</name><sort-name>United States</sort-name><id>489ce91b-6658-3307-9877-795b68554c98</id><disambiguation></disambiguation></area><type>Group</type><gender /><name>Nirvana</name><aliases><type /><primary /><locale /><sort-name>Nirvana US</sort-name><name>Nirvana US</name></aliases><life-span><begin>1988-01</begin><end>1994-04-05</end><ended>true</ended></life-span><disambiguation>90s US grunge band</disambiguation><end_area /><sort-name>Nirvana</sort-name><country>US</country><id>5b11f4ce-a62d-471e-81fc-a69a8278c7da</id><begin_area><name>Aberdeen</name><id>a640b45c-c173-49b1-8030-973603e895b5</id><sort-name>Aberdeen</sort-name><disambiguation></disambiguation></begin_area></root>";

                ResponseConverter responseConverter = new ResponseConverter();

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(xmlData, Encoding.Unicode, mimeType);

                //Act
                var actual = responseConverter.GetXmlFromResponse(response).Result;

                //Assert

                //No exceptions thrown
            });
        }
        [Test]
        public void CanNOT_readXML_fromApplication_TEXT_Response_MIMETypes()
        {

            Assert.Throws<InvalidDataException>(() =>
            {
                //Arrange

                String mimeType = "application/csv";
                //Data retrieved from: http://musicbrainz.org/ws/2/artist/5b11f4ce-a62d-471e-81fc-a69a8278c7da?inc=aliases&fmt=json
                String csvData = "name,life-span-begin life-span-end,disambiguation,country\nNirvana,1988-01,1994-04-05,90s US grunge band,US";

                ResponseConverter responseConverter = new ResponseConverter();

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(csvData, Encoding.Unicode, mimeType);

                //Act
                try
                {
                    responseConverter.GetXmlFromResponse(response).Wait();
                }
                catch (AggregateException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw;
                }

                //Assert
                
            });
        }



    }
}
