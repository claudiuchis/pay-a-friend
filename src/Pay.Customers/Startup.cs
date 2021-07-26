using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Eventuous;
using MongoDB.Driver;
using EventStore.Client;
using Eventuous.Subscriptions.EventStoreDB;
using Eventuous.Projections.MongoDB;
using Eventuous.EventStoreDB;

using Pay.Verification.Projections;

namespace Pay.Verification
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
            services
                .AddControllers();

            services
                .AddCustomAuthorization()
                .AddEventStore(Configuration["EventStore"])
                .AddMongoStore(Configuration["MongoDB"])
                .AddCustomServices()
                .AddProjections();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });
        }
    }

    public static class StartupExtensions
    {

        public static IServiceCollection AddCustomAuthorization(
            this IServiceCollection services
        )
        {
            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            // adds an authorization policy to make sure the token is for the correct scope
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "pay.customers");
                });
            });
            
            return services;
        }

        public static IServiceCollection AddCustomServices(
            this IServiceCollection services
        )
        {
            services
                .AddSingleton<CustomersCommandService>();

            return services;
        }

        public static IServiceCollection AddProjections(
            this IServiceCollection services)
        {
            services
                .AddHostedService<AllStreamSubscription>( provider => {
                    var subscriptionId = "users.projections";
                    var loggerFactory = provider.GetLoggerFactory();

                    return new AllStreamSubscription(
                        provider.GetEventStoreClient(),
                        subscriptionId,
                        new MongoCheckpointStore(
                            provider.GetMongoDatabase(),
                            loggerFactory.CreateLogger<MongoCheckpointStore>()
                        ),
                        new[] { new CustomerProjection(provider.GetMongoDatabase(), subscriptionId, loggerFactory)},
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
            
            var settings = EventStoreClientSettings.Create(eventStoreConnectionString);
            var eventStoreClient = new EventStoreClient(settings);
            var eventStore = new EsdbEventStore(eventStoreClient);
            services.AddSingleton(eventStoreClient);
            var aggregateStore = new AggregateStore(eventStore, DefaultEventSerializer.Instance);
            services.AddSingleton<IAggregateStore>(aggregateStore);
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
        public static IAggregateStore GetAggregateStore(this IServiceProvider provider)
            => provider.GetRequiredService<IAggregateStore>();
        public static IMongoDatabase GetMongoDatabase(this IServiceProvider provider)
            => provider.GetRequiredService<IMongoDatabase>();
        public static EventStoreClient GetEventStoreClient(this IServiceProvider provider)
            => provider.GetRequiredService<EventStoreClient>();

    }
}
