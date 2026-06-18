using DS.Presentation.DI;
using DS.Presentation.Middlewares;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;

namespace DS.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Srarting web server");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors();

            builder.Services.AddLogging(builder.Configuration);
            builder.Services.AddWebDI();
            builder.Services.AddInfrastructureDI();
            builder.Services.AddApplicationDI();

            var app = builder.Build();


            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:3000", "https://localhost:7242")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });

            app.UseExceptionMiddleware();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = "openapi/{documentName}.json";
                });
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "Scalar Example";
                    opt.Theme = ScalarTheme.Mars;
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Server dead");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
