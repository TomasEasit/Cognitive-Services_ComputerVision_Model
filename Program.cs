using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Cognitive_Services_ComputerVision_Model
{
    class Program
    {

        // SETUP fields: Remember to move the API Keys and secrets to a safe place
        static string subscriptionKey = "subscriptionKey";                                  /* Api Key you get from MS Cognitive Services */
        const string endpoint = "https://northeurope.api.cognitive.microsoft.com/";         /* Enpoint, when you create API key you cet this */
        static string uriBase = endpoint + "vision/v2.0/analyze";                           /* The Analyze method endpoint, can be 1.0 eller 2.0 */
        static string imageFilePath = @"C:\Image.jpg";                                      /* Placeholder where the image you want to analyze is. */

        static void Main(string[] args)
        {
            // Run the analyze and send in the path where the file is.
            AnalyzeImage(imageFilePath);
        }

        public static async void AnalyzeImage(string aImageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // There is more parameters that can be used than this, but to fit the model i only use Description
            string requestParameters = "visualFeatures=Description";

            // Building the parameters choosed above.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Creates a byteArray from the choosen image
            byte[] byteData = GetImageAsByteArray(aImageFilePath);

            // Add the byte array as an octet stream to the request body.
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses the "application/octet-stream" content type.
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Sending the array to Cognitive Services Api
                response = await client.PostAsync(uri, content);
            }

            // Gets the answer back as JSON
            string contentString = await response.Content.ReadAsStringAsync();


            // Creating a ImageObject from the JSON answer..
            var ResponseObject = JsonConvert.DeserializeObject<ImageObject>(contentString);

            // To get the analyze confidence % use:
            //
            // ResponseObject.description.captions[0].confidence.ToString();
            
            // To get the analyze image description use:
            //
            // ResponseObject.description.captions[0].text.ToString();
            
            // To get the image tags and put it in a list you can make a foreach loop:
            //
            // foreach (var item in ResponseObject.description.tags)
            // {            
            //      ImageTagList.Add(item.ToString());
            // }


            // Do stuff with Object!
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file. and creates a byteArray so it can be sent to be 
            // analyzad by cognitive Services

            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }



        // Classes needed to create ImageObject from the Json answer. 
        public class ImageObject
        {
            public Description description { get; set; }
            public string requestId { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Caption
        {
            public string text { get; set; }
            public double confidence { get; set; }
        }

        public class Description
        {
            public List<string> tags { get; set; }
            public List<Caption> captions { get; set; }
        }

        public class Metadata
        {
            public int height { get; set; }
            public int width { get; set; }
            public string format { get; set; }
        }
    }
}
