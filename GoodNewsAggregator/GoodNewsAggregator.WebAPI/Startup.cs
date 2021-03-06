using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.IO;
using System.Reflection;
using System.Text;
using GoodNewsAggregator.Core.Services.Implementation;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.QueryHandlers.Articles;
using GoodNewsAggregator.Tools;
using GoodNewsAggregator.WebAPI.Auth;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GoodNewsAggregator.WebAPI
{
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
            services.AddControllersWithViews();

            services.AddScoped<IArticleService, ArticleCqrsService>();
            services.AddScoped<IUserService, UserCqrsService>();
            services.AddScoped<IRssService, RssCqrsService>();
            services.AddScoped<ICommentService, CommentCqrsService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenCqrsService>();
            services.AddScoped<IJwtAuthManager, JwtAuthManager>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(typeof(AutoMap).Assembly);

            services.AddMediatR(typeof(GetArticleByIdHandler).GetTypeInfo().Assembly);

            services.AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions()
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(30),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                })
            );
            services.AddHangfireServer();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<GoodNewsAggregatorContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(opt =>
            {
                opt.Audience = Configuration["Jwt:Audience"];
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddDbContext<GoodNewsAggregatorContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodNewsAggregator.WebAPI", Version = "v1" });

                var xmlPath = Path.Combine($"{Assembly.GetExecutingAssembly().GetName().Name}.XML");

                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsAggregator.WebAPI v1"));

            app.UseHangfireDashboard();
            var articleService = serviceProvider.GetService(typeof(IArticleService)) as IArticleService;

            uint rateTime, aggregateTime;

            if (!UInt32.TryParse(Configuration["Hangfire:Rate"], out rateTime))
                rateTime = 15;

            if (!UInt32.TryParse(Configuration["Hangfire:Aggregate"], out aggregateTime))
                aggregateTime = 30;

            RecurringJob.AddOrUpdate(() => articleService.AggregateNews(), $"*/{aggregateTime} * * * *");
            RecurringJob.AddOrUpdate(() => articleService.RateNews(), $"*/{rateTime} * * * *");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("Default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
