using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Shared;
using System.IO;
using System.Linq;
using WebApiContrib.Core.Formatter.Protobuf;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //---Compression Config---
            //https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?tabs=aspnetcore2x
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/x-protobuf" });
            });
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddMvc()
                //---Protobuf Config---
                //https://github.com/WebApiContrib/WebAPIContrib.Core/tree/master/src/WebApiContrib.Core.Formatter.Protobuf
                .AddProtobufFormatters()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            //---Swagger Config---
            //https://github.com/domaindrivendev/Swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                foreach(var filePath in FindXmlFiles(PlatformServices.Default.Application.ApplicationBasePath))
                //var xmlPath = Path.Combine(basePath, "TodoApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Logs Responses to a file
            //app.UseMiddleware<LoggingMiddleware>();

            if (env.IsDevelopment())  app.UseDeveloperExceptionPage();

            //---Compression Config---
            //Note: Must be BEFORE UseMvc()
            app.UseResponseCompression();

            //---Swagger Config---
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();

            //Preload the RandomWords list
            var t = RandomWords.Words;
        }

        /// <summary>
        /// Finds xml files in the given path
        /// </summary>
        private string[] FindXmlFiles(string basePath)
        {
            var files = Directory.GetFiles(basePath, "*.xml");
            return files;
        }

    }
}
