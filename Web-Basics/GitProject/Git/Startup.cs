namespace Git
{
    using System.Threading.Tasks;
    using CarShop.Services;
    using Git.Data;
    using Git.Services;
    using Microsoft.EntityFrameworkCore;
    using MyWebServer;
    using MyWebServer.Controllers;
    using MyWebServer.Results.Views;

    public class Startup
    {


        public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IValidator,Validator>()
                    .Add<IPasswordHasher,PasswordHasher>()
                    .Add<IViewEngine, CompilationViewEngine>()
                    .Add<GitDbContext>())
                .Start();
    }
}
