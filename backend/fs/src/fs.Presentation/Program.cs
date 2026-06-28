using fs.Presentation.DI;
using FS.Infrastructure.Postgres;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;
using FS.Infrastructure.S3;
using FS.Core;

namespace fs.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
           .CreateBootstrapLogger();

        LoadEnvFile();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLogging();

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddCore();
        builder.Services.AddInfrastructurePostgres(builder.Configuration);
        builder.Services.AddS3Infrastructure(builder.Configuration);
        builder.Services.AddWebDI();

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();



        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    static void LoadEnvFile()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null)
        {
            var path = Path.Combine(dir.FullName, ".env");
            if (File.Exists(path))
            {
                DotNetEnv.Env.Load(path);
                Console.WriteLine($"Загружен .env из: {path}");
                return;
            }
            dir = dir.Parent;
        }
        Console.WriteLine(".env файл не найден");
    }
}
