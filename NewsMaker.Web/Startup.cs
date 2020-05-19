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
using NewsMaker.Web.Configuration.Mapper;
using NewsMaker.Web.IntegrationEvents;
using NewsMaker.Web.Services;
using Infrastructure.Data.Infrastructure;
using Infrastructure.Data;

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
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddSingleton<SearchEngine>();
            RegistryAutoMapper(services);
            RegistryServiceBus(services);


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            services.AddEntityFrameworkSqlServer()
                .AddDbContext<NewsContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
                });


            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
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
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<EventBusRabbitMQ>();

            eventBus.Subscribe<NewsAddEvent, NewsAddEventHandler>();
            eventBus.Subscribe<NewsRemoveEvent, NewsRemoveEventHandler>();
            eventBus.Subscribe<NewsUpdateEvent, NewsUpdateEventHandler>();
        }
    }
}
