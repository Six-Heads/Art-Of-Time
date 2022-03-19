using ArtOfTime.Data;
using ArtOfTime.Data.Repositories.Contracts;
using ArtOfTime.Data.Repositories.Implementations;
using ArtOfTime.Interfaces;
using ArtOfTime.Jobs;
using ArtOfTime.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ArtOfTime
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(c => c.UseMemoryStorage());

            JobStorage.Current = new MemoryStorage();

            RecurringJob.RemoveIfExists("generateimage");
            RecurringJob.AddOrUpdate<GenerateImageJob>("generateimage", x => x.Work(null), Cron.Minutely);

            services.AddRazorPages();

            services.AddTransient<IImageRepository, ImageRepository>();

            services.AddTransient<IApiProvider, ApiProvider>();
            services.AddTransient<IIPFSService, IPFSService>();
            services.AddTransient<IEthereumService, EthereumService>();
            services.AddTransient<IImageGeneratorService, ImageGeneratorService>();
            services.AddTransient<ITwitterService, TwitterService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            app.UseHangfireServer();

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

            context.Database.Migrate();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
