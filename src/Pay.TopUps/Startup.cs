using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Eventuous;
using MongoDB.Driver;
using Eventuous.Subscriptions.EventStoreDB;
using Eventuous.Projections.MongoDB;
using Eventuous.EventStoreDB;
using Stripe;

using Pay.TopUps.Projections;
using Pay.TopUps.StripePayments;

namespace Pay.TopUps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DotNetEnv.Env.Load();
            StripeConfiguration.ApiKey = System.Environment.GetEnvironmentVariable("SK_TEST_KEY");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "App.Topups", Version = "v1" });
            });

            services
                .AddEventStore(Configuration["EventStore"])
                .AddMongoStore(Configuration["MongoDB"])
                .AddCustomServices()
                .AddProjections();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App.Topups v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class StartupExtensions
    {
        public static IServiceCollection AddCustomServices(
            this IServiceCollection services
        )
        {
            services
                .AddSingleton<StripeWebhookService>();

            return services;
        }

        public static IServiceCollection AddProjections(
            this IServiceCollection services)
        {
            services
                .AddHostedService<StreamSubscription>( provider => {
                    var subscriptionId = "topups.projections";
                    var loggerFactory = provider.GetLoggerFactory();

                    return new StreamSubscription(
                        provider.GetEventStoreClient(),
                        StreamNames.TopUpsStream,
                        subscriptionId,
                        new MongoCheckpointStore(
                            provider.GetMongoDatabase(),
                            loggerFactory.CreateLogger<MongoCheckpointStore>()
                        ),
                        new[] { new TopUpProjection(provider.GetMongoDatabase(), subscriptionId, loggerFactory)},
                        DefaultEventSerializer.Instance,
                        loggerFactory
                    );

                });

            return services;
        }        
        public static IServiceCollection AddEventStore(
            this IServiceCollection services,
            string eventStoreConnectionString
        )
        {
            EventMapping.MapEventTypes();
            
            services
                .AddSingleton( sp => {
                    var settings = EventStore.Client.EventStoreClientSettings.Create(eventStoreConnectionString);
                    return new EventStore.Client.EventStoreClient(settings);
                })
                .AddSingleton<IEventStore>( sp => {
                    return new EsdbEventStore(sp.GetEventStoreClient());
                })
                .AddSingleton<IAggregateStore>(sp => {
                    return new AggregateStore(sp.GetEventStore(), DefaultEventSerializer.Instance);
                });

            return services;
        }

        public static IServiceCollection AddMongoStore(
            this IServiceCollection services,
            string mongoDBConnectionString
        )
        {
            var mongoClient = new MongoClient(mongoDBConnectionString);
            var database = mongoClient.GetDatabase("readside");
            services.AddSingleton<IMongoDatabase>(database);
            return services;
        }        
        public static ILoggerFactory GetLoggerFactory(this IServiceProvider provider)
            => provider.GetRequiredService<ILoggerFactory>();
        public static IEventStore GetEventStore(this IServiceProvider provider)
            => provider.GetRequiredService<IEventStore>();
        public static IAggregateStore GetAggregateStore(this IServiceProvider provider)
            => provider.GetRequiredService<IAggregateStore>();
        public static IMongoDatabase GetMongoDatabase(this IServiceProvider provider)
            => provider.GetRequiredService<IMongoDatabase>();
        public static EventStore.Client.EventStoreClient GetEventStoreClient(this IServiceProvider provider)
            => provider.GetRequiredService<EventStore.Client.EventStoreClient>();

    }
}
