using Microsoft.OData.JsonLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;

namespace TestApp
{
    public class ODataTest
    {
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntityType<string>();
            //builder.EntitySet<string>("value");
            //builder.EntitySet<string>("@odata.nextLink");
            
            return builder.GetEdmModel();
        }

        public static async Task Main2(string[] args)
        {
            Stream stream = new MemoryStream(Encoding.GetEncoding("iso-8859-1").GetBytes("{\"@odata.context\":\"http://stuff/#Edm.Int32\",\"value\":4}"));
            ODataMessageReaderSettings settings = new ODataMessageReaderSettings();
            ODataMessageInfo messageInfo = new ODataMessageInfo();
         ///   var oDataJsonLightInputContext = new ODataJsonLightInputContext(messageInfo: messageInfo, settings);
        }

        public static async Task Main1(string[] args)
        {
            String path = @"C:\Users\kibandi\Desktop\SimpleExchangeOutput.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                IEdmModel model = null;
                /*model = builder
                    .BuildAddressType()
                    .BuildCategoryType()
                    .BuildCustomerType()
                    .BuildDefaultContainer()
                    .BuildCustomerSet()
                    .GetModel();*/

                Stream stream = sr.BaseStream;
                ODataMessageReaderSettings settings = new ODataMessageReaderSettings();
                IODataResponseMessage responseMessage = new InMemoryMessage { Stream = stream };
                responseMessage.SetHeader("Content-Type", "application/json;odata.metadata=minimal;");
                // ODataMessageReader reader = new ODataMessageReader((IODataResponseMessage)message, settings, GetEdmModel());
                ODataMessageReader reader = new ODataMessageReader(responseMessage, settings, new EdmModel());
                var oDataResourceReader = reader.CreateODataResourceReader();
                var property = reader.ReadProperty();

                //ODataStreamInfo odataStream = reader.Item as ODataStreamInfo;

                stream.Position = 0;
                //var asynchronousReader = reader.CreateODataAsynchronousReader();
                IEdmStructuredType resource = new EdmComplexType("odata", "odata");
                oDataResourceReader = reader.CreateODataResourceReader(resourceType: resource);
                while (oDataResourceReader.Read())
                {
                    var oItem = oDataResourceReader.Item;
                    Console.WriteLine(oItem.ToString());
                }
                //var responseMessage = asynchronousReader.CreateResponseMessage();

            }
        }
    }
}
