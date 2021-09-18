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
using IdentityServer4;

using MongoDB.Driver;
using static BCrypt.Net.BCrypt;

using Eventuous;
using Eventuous.Subscriptions;
using Eventuous.Subscriptions.EventStoreDB;
using Eventuous.Projections.MongoDB;
using Eventuous.EventStoreDB;

using Pay.Identity.Registration;
using Pay.Identity.Authentication;
using Pay.Identity.Reactions;
using Pay.Identity.Projections;
using Pay.Identity.Configs;
using Pay.Identity.Domain.Emails;
using Pay.Identity.Infrastructure;
using Pay.Common;

namespace Pay.Identity
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

            services
                .Configure<SendgridConfiguration>(Configuration.GetSection("Sendgrid"))
                .Configure<ReferenceUrls>(Configuration.GetSection("ReferenceUrls"))
                .AddCustomIdentityServer()
                .AddEventStore(Configuration["EventStore"])
                .AddMongoStore(Configuration["MongoDB"])
                .AddCustomServices()
                .AddReactions()
                .AddProjections()
                .AddEventStoreProjections(Configuration["EventStore"])
            ;
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class StartupExtensions
    {
        /*
        For local development/testing, to get the browser to recognize the self-signed certificate, 
        and thus to be able to login using the Pay.Identity service), run this command: 
            dotnet dev-certs https --trust
        
        More here:
        https://www.thesslstore.com/blog/how-to-make-ssl-certificates-play-nice-with-asp-net-core/
        */
        public static IServiceCollection AddCustomIdentityServer(
            this IServiceCollection services
        )
        {
            var builder = services.AddIdentityServer(options => 
                options.UserInteraction.LoginUrl = "/Authentication/Login"
                )
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients);

            builder.AddDeveloperSigningCredential();
            return services;
        }
        public static IServiceCollection AddCustomServices(
            this IServiceCollection services
        )
        {
            services
                .AddSingleton(
                    c => 
                    {
                        return new RegistrationService(
                            c.GetAggregateStore(),
                            (plain) => HashPassword(plain),
                            c.GetRequiredService<ISendEmailService>()
                        );
                    }
                )
                .AddSingleton(
                    c => 
                    {
                        return new AuthenticationService(
                            c.GetMongoDatabase(),
                            (plainPassword, hashedPassword) => Verify(plainPassword, hashedPassword)
                        );
                    }
                )
                .AddSingleton<ISendEmailService, SendGridService>();
            return services;
        }

        public static IServiceCollection AddReactions(
            this IServiceCollection services)
        {
            services
                .AddSingleton<IHostedService, StreamSubscription>( provider => {
                    var subscriptionId = "user.registrations";
                    var loggerFactory = provider.GetLoggerFactory();

                    return new StreamSubscription(
                        provider.GetEventStoreClient(),
                        new StreamSubscriptionOptions() {
                            StreamName = StreamNames.UserRegistrationsStream,
                            SubscriptionId = subscriptionId,
                            ThrowOnError = true,
                            ResolveLinkTos = true
                        },
                        new EsCheckpointStore(
                            provider.GetEventStoreClient(),
                            loggerFactory.CreateLogger<EsCheckpointStore>()
                        ),
                        new IEventHandler[] { 
                            new UserReactions(
                                subscriptionId, 
                                provider.GetRequiredService<RegistrationService>()
                            )
                        },
                        DefaultEventSerializer.Instance,
                        loggerFactory
                    );
                });
            return services;
        }

        public static IServiceCollection AddProjections(
            this IServiceCollection services)
        {
            services
                .AddSingleton<IHostedService, AllStreamSubscription>( provider => {
                    var subscriptionId = "users.projections";
                    var loggerFactory = provider.GetLoggerFactory();

                    return new AllStreamSubscription(
                        provider.GetEventStoreClient(),
                        subscriptionId,
                        new MongoCheckpointStore(
                            provider.GetMongoDatabase(),
                            loggerFactory.CreateLogger<MongoCheckpointStore>()
                        ),
                        new[] { new UserDetailsProjection(provider.GetMongoDatabase(), subscriptionId, loggerFactory)},
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
            
            var settings = EventStore.Client.EventStoreClientSettings.Create(eventStoreConnectionString);
            var eventStoreClient = new EventStore.Client.EventStoreClient(settings);
            var eventStore = new EsdbEventStore(eventStoreClient);
            services.AddSingleton(eventStoreClient);
            var aggregateStore = new AggregateStore(eventStore, DefaultEventSerializer.Instance);
            services.AddSingleton<IAggregateStore>(aggregateStore);
            return services;
        }

        public static IServiceCollection AddEventStoreProjections(
            this IServiceCollection services,
            string eventStoreConnectionString
        )
        {
            ProjectionMapping.MapProjections();
            var settings = EventStore.Client.EventStoreClientSettings.Create(eventStoreConnectionString);
            var eventStoreProjectionClient = new EventStore.Client.EventStoreProjectionManagementClient(settings);
            EsProjectionMap.UpsertProjections(eventStoreProjectionClient);
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
        public static EventStore.Client.EventStoreClient GetEventStoreClient(this IServiceProvider provider)
            => provider.GetRequiredService<EventStore.Client.EventStoreClient>();

    }
}
