using ArtOfTime.Interfaces;
using ArtOfTime.Jobs;
using ArtOfTime.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.MemoryStorage;
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
            services.AddHangfire(c => c.UseMemoryStorage());

            JobStorage.Current = new MemoryStorage();

            var generateJob = new GenerateImageJob();
            RecurringJob.RemoveIfExists("generateimage");
            RecurringJob.AddOrUpdate("generateimage", () => generateJob.Test(null), Cron.Minutely);

            services.AddRazorPages();

            services.AddTransient<IApiProvider, ApiProvider>();
            services.AddTransient<IIPFSService, IPFSService>();
            services.AddTransient<IEthereumService, EthereumService>();
            services.AddTransient<IImageGeneratorService, ImageGeneratorService>();
            services.AddSingleton<ITwitterService, TwitterService>();

            ITwitterService twitterService = new TwitterService(new ApiProvider(), Configuration);
            twitterService.GetTrends();

            IEthereumService ethereumService = new EthereumService(Configuration);
            IIPFSService iPFSService = new IPFSService(new ApiProvider(), Configuration, ethereumService);
            byte[] image = File.ReadAllBytes("test.png");
            System.Console.WriteLine(iPFSService.Upload(image, "1647696027", new System.Collections.Generic.List<string> { "Ukraine", "Queen", "poverty","NFT", "Trump" }).GetAwaiter().GetResult());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
