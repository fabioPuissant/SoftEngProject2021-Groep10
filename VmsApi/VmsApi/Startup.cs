using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VmsApi.Data.DataDbContext;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.Data.Repositories.interfaces;
using VmsApi.Mappers;
using VmsApi.Services;
using VmsApi.Utils;

namespace VmsApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Database Connection SetUp
            services.AddDbContext<VmsDbContext>(
                opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("DBContext")));

            // Identity Service SetUp
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<VmsDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });


            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("*");
            //        });
            //});

            // Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Authentication JWT SetUp
            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.GetSection("secretKey").Value))
                    };
                }
            });

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            // Anti JSON looping
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Custom services
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IMappings, Mappings>();
            services.AddScoped<ITokenGenerator, JwtBearerTokenGenerator>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            services.AddScoped<IFoodPurchasesRepo, FoodPurchaseRepo>();
            services.AddScoped<IMeasurePointRepo, MeasurePointRepo>();
            services.AddScoped<IPigGroupRepo, PigGroupRepo>();

            // In production, the React files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "VmsWeb/build";
            //});

            services.AddTransient<VmsDbContext>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseSpaStaticFiles();
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

            // Required for Authentication!!
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "VmsWeb";
            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});

            // Do not await!
            //  VmsDbContext context, IServiceProvider provider
            //new SeedUserData(context, provider).Initialize();
        }
    }
}