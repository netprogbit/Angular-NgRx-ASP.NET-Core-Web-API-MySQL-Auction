using DataLayer.DbContexts;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Server.Helpers;
using Server.Hubs;
using Server.Middlewares;
using Server.Services;
using Server.Settings;
using System.IO;

namespace Server
{
    public class Startup
    {
        protected readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            StaticConfig = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; } // For use by static classes

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtHelper.Issuer,
                    ValidAudience = JwtHelper.Audience,
                    IssuerSigningKey = JwtHelper.GetSymmetricSecurityKey()
                };
            });

            services.AddDbContext<AuctionDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(Configuration["AppSettings:UrlClient"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseHsts();
            }

            // HTTP error handling
            app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
            app.Map("/error", ap => ap.Run(async context =>
            {
                var code = context.Request.Query["code"];
                _logger.LogError(StringHelper.HttpErrorStatus + code);
                await context.Response.WriteAsync($"Err: {code}");
            }));

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            // Use folder node_modules
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<AuctionHub>("/auction");
            });
            app.UseMvc();

            FileHelper.HostingEnvironment = env; // Set hosting environment for file helper
        }
    }
}
