using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorFluentUI;
using Blazored.Toast;
using MatchingGame.Client.Models;
using MatchingGame.Client.Handler;
using MatchingGame.Client.Services;

namespace MatchingGame.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddBlazorFluentUI();

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            AddHttpClients(builder);

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredToast();

            builder.Services.AddLogging(logging =>
            {
                var httpClient = builder.Services.BuildServiceProvider().GetRequiredService<HttpClient>();
                var authenticationStateProvider = builder.Services.BuildServiceProvider().GetRequiredService<AuthenticationStateProvider>();
                logging.SetMinimumLevel(LogLevel.Error);
                //logging.ClearProviders();
            });


            await builder.Build().RunAsync();
        }

        public static void AddHttpClients(WebAssemblyHostBuilder builder)
        {
            //transactional named http clients
            /* builder.Services.AddHttpClient<IProfileViewModel, ProfileViewModel>
                 ("ProfileViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                 .AddHttpMessageHandler<CustomAuthorizationHandler>();*/

            builder.Services.AddHttpClient<IUserService, UserService>
                 ("UserService", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            //builder.Services.AddHttpClient<ISettings, Settings>
            //    ("SettingsViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //    .AddHttpMessageHandler<CustomAuthorizationHandler>();

            //authentication http clients
            //builder.Services.AddHttpClient<ILogin, Login>
            //    ("LoginViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            //builder.Services.AddHttpClient<IRegister, Register>
            //    ("RegisterViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            //builder.Services.AddHttpClient<IFacebookAuthViewModel, FacebookAuth>
            //    ("FacebookAuthViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            //builder.Services.AddHttpClient<ITwitter, Twitter>
            //   ("TwitterAuthViewModelClient", cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
        }
    }
}