using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace WebApi
{
    /// <summary>
    /// Logs the Response to a file
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Note: Response Body is a write only stream so is clunky to get a hold of
            //https://stackoverflow.com/questions/25584626/response-body-for-request-response-logging
            Stream stream = context.Response.Body;
            MemoryStream responseBuffer = new MemoryStream();
            context.Response.Body = responseBuffer;

            await _next.Invoke(context);

            responseBuffer.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBuffer).ReadToEnd();

            responseBuffer.Seek(0, SeekOrigin.Begin);
            await responseBuffer.CopyToAsync(stream);

            try
            {
                //Write the output to a file
                var fileName = context.Response.Headers.ContentLength + " " + context.Request.Path + " " + ((FrameRequestHeaders)context.Request.Headers).HeaderAccept + " " + DateTime.Now.ToString("dd hh:mm:ss.fff");
                fileName = CleanFileName(fileName);
                await WriteToFile(responseBody, fileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Logging Response! " + ex);
            }
        }

        private async Task WriteToFile(string content, string fileName)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var directory = Path.Combine(basePath, "RequestLogs");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, fileName);

            await File.WriteAllTextAsync(filePath, content);
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "^"));
        }
    }
}
