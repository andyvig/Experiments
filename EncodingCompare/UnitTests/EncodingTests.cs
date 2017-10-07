using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Shared;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    /// <summary>
    /// Tests for different types of encoding.
    /// Uses the In-Memory TestServer currently but can also call a hosted endpoint.
    /// </summary>
    public class EncodingTests
    {
        private readonly EncodingTestHelper helper = new EncodingTestHelper();
        //Xunit uses this for test output
        private readonly ITestOutputHelper output;
        //In-Memory server testing https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing
        private readonly TestServer _server;

        public EncodingTests(ITestOutputHelper output)
        {
            this.output = output;
            RandomWords.WordsFileLocation = @"..\..\..\..\words.json";

            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<WebApi.Startup>());
        }

        [Theory]
        [InlineData(10)]
        [InlineData(300)]
        //[InlineData(5000)]
        public async Task TestEncoding(int payloadRowCount)
        {
            //Set the HttpClient
            //helper.client = new HttpClient(); //Requires the actual server be running
            helper.client = _server.CreateClient();

            //Uncompressed
            await CompareEncoding<MixedPayload[]>(payloadRowCount, "mixed", false);
            await CompareEncoding<NumericPayload[]>(payloadRowCount, "numeric", false);
            await CompareEncoding<FacebookPayload[]>(payloadRowCount, "facebook", false);
            //Compressed
            await CompareEncoding<MixedPayload[]>(payloadRowCount, "mixed", true);
            await CompareEncoding<NumericPayload[]>(payloadRowCount, "numeric", true);
            await CompareEncoding<FacebookPayload[]>(payloadRowCount, "facebook", true);
        }

        //Compares 3 different types of encoding and their payload size
        private async Task CompareEncoding<T>(int count, string endpoint, bool useCompression)
        {
            output.WriteLine($"---Comparing {typeof(T).Name} Encoding---");
            output.WriteLine($"Count={count} Compressed={useCompression}");

            var xmlResult = await helper.CallServerAsync<T>(count, endpoint, "application/xml", useCompression);
            var jsonResult = await helper.CallServerAsync<T>(count, endpoint, "application/json", useCompression);
            var protoResult = await helper.CallServerAsync<T>(count, endpoint, "application/x-protobuf", useCompression);

            output.WriteLine($"Xml length {xmlResult.length}");
            output.WriteLine($"Json length {jsonResult.length}");
            output.WriteLine($"Protobuf length {protoResult.length}");
        }

        
    }
}
