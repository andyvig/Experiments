using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnitTests
{
    public class EncodingTestHelper
    {
        public const int Port = 2241;
        public HttpClient client = null;

        //Calls the server endpoint with given encoding
        public async Task<(int length, string contentType, T results)>
            CallServerAsync<T>(
                int count, string endpoint, string encodingHeader, bool requestCompressed
            )
        {
            if (client == null)
                throw new InvalidOperationException("client must be set");

            //Note: Content-Length isn't acurate if we have the HttpClient automatically decompress for us,
            //so in our case we do it manually.  https://stackoverflow.com/questions/28754673/httpclient-conditionally-set-acceptencoding-compression-at-runtime
            //var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            if (requestCompressed)
            {
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            }

            //Call the endpoint with the specified encoding
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:{Port}/api/payloadtest/{endpoint}/{count}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(encodingHeader));
            var result = await client.SendAsync(request);
            //Grab some header info
            var length = (int)result.Content.Headers.ContentLength.Value;
            var contentType = result.Content.Headers.ContentType.ToString();
            var encoding = result.Content.Headers.ContentEncoding.ToString();

            //Deserialize
            if (contentType.Contains("json"))
            {
                return (length, contentType, JsonConvert.DeserializeObject<T>(await GetContent(result.Content, requestCompressed)));
            }
            else if (contentType.Contains("proto"))
            {
                return (length, contentType, ProtoBuf.Serializer.Deserialize<T>(await GetStream(result.Content, requestCompressed)));
            }
            else if (contentType.Contains("xml"))
            {
                var deserializer = new XmlSerializer(typeof(T));
                return (length, contentType, (T)deserializer.Deserialize(await GetStream(result.Content, requestCompressed)));
            }
            else
            {
                throw new InvalidOperationException("Unexpected content type " + contentType);
            }
        }

        //Gets a content stream, and uncompresses the contents if necessary
        private async Task<Stream> GetStream(HttpContent content, bool isCompressed)
        {
            if (!isCompressed)
                return await content.ReadAsStreamAsync();

            return new GZipStream(await content.ReadAsStreamAsync(), CompressionMode.Decompress);
        }

        //Gets the content as a string and uncompresses the contents if necessary
        private async Task<string> GetContent(HttpContent content, bool isCompressed)
        {
            var output = "notSet";
            if (isCompressed)
            {
                using (Stream stream = await content.ReadAsStreamAsync())
                using (Stream decompressed = new GZipStream(stream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(decompressed))
                {
                    output = reader.ReadToEnd();
                }
            }
            else
            {
                output = await content.ReadAsStringAsync();
            }
            return output;
        }
    }
}
