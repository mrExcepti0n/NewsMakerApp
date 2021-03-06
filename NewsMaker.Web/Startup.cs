using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Infrastructure.EventBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Reflection;
using NewsMaker.Web.Configuration.Mapper;
using NewsMaker.Web.IntegrationEvents;
using NewsMaker.Web.Services;
using Infrastructure.Data;
using Nest;
using Domain.Core.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace NewsMaker.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSingleton<SearchEngine>();
            RegistryAutoMapper(services);
            RegistryServiceBus(services);
            services.AddElasticsearch(Configuration);

            // Add framework services.
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NewsMaker HTTP API",
                    Version = "v1",
                    Description = "NewsMaker HTTP API"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<NewsContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
                });
        }


        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac
        }

        private void RegistryServiceBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];


            services.AddSingleton<RabbitMQConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    UserName = Configuration["EventBusUserName"],
                    Password = Configuration["EventBusPassword"]
                   
                };

                return new RabbitMQConnection(factory);
            });

            services.AddSingleton<EventBusRabbitMQ>();

            services.AddTransient<NewsAddEventHandler>();
            services.AddTransient<NewsRemoveEventHandler>();
            services.AddTransient<NewsUpdateEventHandler>();

        }

        private void RegistryAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new NewsServiceProfiler());
                mc.AddProfile(new NewsServiceProfiler());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
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

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            var newsContext = app.ApplicationServices.GetRequiredService<NewsContext>();
            newsContext.Database.Migrate();
            new NewsContextSeed().Seed(newsContext);

            ConfigureEventBus(app);

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsMaker API V1");
                });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<EventBusRabbitMQ>();

            eventBus.Subscribe<NewsAddEvent, NewsAddEventHandler>();
            eventBus.Subscribe<NewsRemoveEvent, NewsRemoveEventHandler>();
            eventBus.Subscribe<NewsUpdateEvent, NewsUpdateEventHandler>();
        }
    }

    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch( this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticsearchConnection:url"];
            var defaultIndex = configuration["ElasticsearchConnection:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<News>(m => m
                                            .IndexName("news"));

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }
    }
}
