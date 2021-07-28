using MatchingGame.Server.DAL;
using MatchingGame.Server.Helpers;
using MatchingGame.Server.Hubs;
using MatchingGame.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;

namespace MatchingGame.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();
            services.AddDbContext<Context>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddAuthentication(options =>
            {
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddCookie(options => { options.LoginPath = "/user/notauthorized"; })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.RequireHttpsMetadata = true;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecrete"];
                twitterOptions.RetrieveUserDetails = true;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecrete"];
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<GameManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            var appDBContext = serviceProvider.GetRequiredService<Context>();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<UnoVsUnoHub>("/uno_vs_uno_hub");
                endpoints.MapHub<TorneoHub>("/torneo_hub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
