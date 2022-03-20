using ArtOfTime.Data;
using ArtOfTime.Data.Repositories.Contracts;
using ArtOfTime.Data.Repositories.Implementations;
using ArtOfTime.Interfaces;
using ArtOfTime.Jobs;
using ArtOfTime.Services;
using Grpc.Net.Client;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using gRPCImageGenerator;
using System;

namespace ArtOfTime.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) 
            => services.AddDbContext<ApplicationDbContext>(options =>                                                                
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
                .AddTransient<IImageRepository, ImageRepository>()
                .AddTransient<IApiProvider, ApiProvider>()
                .AddTransient<IIPFSService, IPFSService>()
                .AddTransient<IEthereumService, EthereumService>()
                .AddTransient<IImageGeneratorService, ImageGeneratorService>()
                .AddTransient<ITwitterService, TwitterService>();

        public static IServiceCollection AddHangfire(this IServiceCollection services)
        {
            services.AddHangfire(c => c.UseMemoryStorage());
            JobStorage.Current = new MemoryStorage();

            RecurringJob.RemoveIfExists("generateimage");
            RecurringJob.AddOrUpdate<GenerateImageJob>("generateimage", x => x.Work(null), Cron.Minutely());
            RecurringJob.Trigger("generateimage");

            return services;
        }

        public static IServiceCollection AddgRPC(this IServiceCollection services)
        {
            services.AddGrpcClient<ImageGenerator.ImageGeneratorClient>(o =>
            {
                o.Address = new Uri("http://8886-2001-67c-20d0-aac-9d0f-48af-6ed4-ad13.ngrok.io/");
            });

            return services;
        }
    }
}
