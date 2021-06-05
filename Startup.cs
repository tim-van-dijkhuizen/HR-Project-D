using FitbyteServer.Database;
using FitbyteServer.Helpers;
using FitbyteServer.Services;
using FitbyteServer.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FitbyteServer {

    public class Startup {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();

            // Register configurations
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

            // Register database service
            services.AddSingleton<IMongoDatabase>(sp => {
                IDatabaseSettings settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                MongoClient client = DatabaseHelper.BuildClient(settings);
                IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

                return database;
            });

            services.AddHostedService<ConfigureMongoIndexes>();

            services.AddSingleton<ProfileService>();
            services.AddSingleton<WorkoutService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<HttpExceptionMiddleware>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

    }

}
