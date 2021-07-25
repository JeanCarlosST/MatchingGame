using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MatchingGame.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using MatchingGame.Client.Logging;
//using Blazored.Toast;
using Blazored.LocalStorage;
using MatchingGame.Client.Handlers;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Blazored.Toast;

namespace MatchingGame.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            AddHttpClients(builder);

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredToast();

            builder.Services.AddTransient<CustomAuthorizationHandler>();

            builder.Services.AddLogging(logging =>
            {
                var httpClient = builder.Services.BuildServiceProvider().GetRequiredService<HttpClient>();
                var authenticationStateProvider = builder.Services.BuildServiceProvider().GetRequiredService<AuthenticationStateProvider>();
                logging.SetMinimumLevel(LogLevel.Error);
                //logging.ClearProviders();
                logging.AddProvider(new ApplicationLoggerProvider(httpClient, authenticationStateProvider));
            });


            await builder.Build().RunAsync();
        }

        public static void AddHttpClients(WebAssemblyHostBuilder builder)
        {
            //transactional named http clients
            /* builder.Services.AddHttpClient<IProfileViewModel, ProfileViewModel>
                 ("ProfileViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                 .AddHttpMessageHandler<CustomAuthorizationHandler>();

             builder.Services.AddHttpClient<ISettingsViewModel, SettingsViewModel>
                 ("SettingsViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                 .AddHttpMessageHandler<CustomAuthorizationHandler>();*/

            //authentication http clients
            builder.Services.AddHttpClient<ILoginViewModel, LoginViewModel>
                ("LoginViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<IRegisterViewModel, RegisterViewModel>
                ("RegisterViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<IFacebookAuthViewModel, FacebookAuthViewModel>
                ("FacebookAuthViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<ITwitterAuthViewModel, TwitterAuthViewModel>
               ("TwitterAuthViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
        }
    }
}