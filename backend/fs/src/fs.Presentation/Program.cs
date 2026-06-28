using fs.Presentation.DI;
using FS.Infrastructure.Postgres;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;

namespace fs.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
               .CreateBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging();

            builder.Services.AddInfrastructurePostgres(builder.Configuration);
            builder.Services.AddWebDI();

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = "openapi/{documentName}.json";
                });
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "File service";
                    opt.Theme = ScalarTheme.Mars;
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
