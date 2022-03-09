using DataLayer.Contexts;
using DataLayer.Entities.Account;
using LogicLayer.Helpers;
using LogicLayer.Hubs;
using LogicLayer.Interfaces;
using LogicLayer.InterfacesOut.Auction;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Services;
using LogicLayer.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Middlewares;
using System;
using System.Text;
using DataLayer.UnitOfWorks.Account;
using DataLayer.UnitOfWorks.Auction;

namespace Server
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            // Set helpers
            ConfigurationHelper.Configuration = Configuration;
            FileHelper.WebRootPath = env.WebRootPath;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Account DB

            var userDbConnectionString = Configuration.GetConnectionString("Account");

            services.AddDbContext<AccountDbContext>(optionsBuilder =>
                optionsBuilder.UseMySql(
                    userDbConnectionString,
                    ServerVersion.AutoDetect(userDbConnectionString)
                )
            );

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AccountDbContext>();

            // Auction DB

            var auctionDbConnectionString = Configuration.GetConnectionString("Auction");

            services.AddDbContext<AuctionDbContext>(optionsBuilder =>
                optionsBuilder.UseMySql(
                    auctionDbConnectionString,
                    ServerVersion.AutoDetect(auctionDbConnectionString)
                )
            );

            services.AddScoped<IAccountUnitOfWork, AccountUnitOfWork>();
            services.AddScoped<IAuctionUnitOfWork, AuctionUnitOfWork>();
            services.AddSingleton<ICacheService, CacheService>(); 
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuctionService, AuctionService>();

            services.AddCors(options =>
            {
                var clientUrl = Configuration["AppSettings:UrlClient"];

                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(clientUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });

            var secret = Configuration["AppSettings:Secret"];
            var key = Encoding.UTF8.GetBytes(secret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PriceHub>("/price");
                endpoints.MapControllers();
            });
        }
    }
}
