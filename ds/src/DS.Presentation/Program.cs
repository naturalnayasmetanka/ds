using DS.Presentation.DI;
using DS.Presentation.Middlewares;
using Scalar.AspNetCore;

namespace DS.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddWebDI();
        builder.Services.AddInfrastructureDI();
        builder.Services.AddApplicationDI();

        var app = builder.Build();

        app.UseExceptionMiddleware();

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
}
