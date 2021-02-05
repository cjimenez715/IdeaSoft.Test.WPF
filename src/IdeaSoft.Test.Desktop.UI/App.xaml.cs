using IdeaSoft.Test.Desktop.UI.Services;
using IdeaSoft.Test.Desktop.UI.ViewModel;
using IdeaSoft.Test.Desktop.UI.Views;
using IdeaSoft.Test.Desktop.UI.Views.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace IdeaSoft.Test.Desktop.UI
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public IConfiguration Configuration { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpClient();
            services.AddScoped<IPersonService, PersonService>();
            
            services.AddSingleton<SearchPersonVw>();
            services.AddScoped<SavePersonVw>();
            services.AddScoped<UpdatePersonVw>();

            services.AddScoped<SearchPersonVm>();
            services.AddScoped<SavePersonVm>();
            services.AddScoped<UpdatePersonVm>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<SearchPersonVw>();
            mainWindow.Show();
        }
    }
}
