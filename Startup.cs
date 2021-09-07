using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphQL;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.Server;

namespace MovieReviews
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
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<MovieContext>(context => { context.UseInMemoryDatabase("MovieDb"); });

            services
            .AddGraphQL(
                (options, provider) =>
                {
                    // Load GraphQL Server configurations
                    var graphQLOptions = Configuration
                        .GetSection("GraphQL")
                        .Get<GraphQLOptions>();
                    options.ComplexityConfiguration = graphQLOptions.ComplexityConfiguration;
                    options.EnableMetrics = graphQLOptions.EnableMetrics;
                    // Log errors
                    var logger = provider.GetRequiredService<ILogger<Startup>>();
                    options.UnhandledExceptionDelegate = ctx =>
                        logger.LogError("{Error} occurred", ctx.OriginalException.Message);
                })
            // Adds all graph types in the current assembly with a singleton lifetime.
            .AddGraphTypes()
            // Add GraphQL data loader to reduce the number of calls to our repository. https://graphql-dotnet.github.io/docs/guides/dataloader/
            .AddDataLoader()
            .AddSystemTextJson();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGraphQLAltair();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
