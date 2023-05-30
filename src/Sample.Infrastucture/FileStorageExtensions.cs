using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Shared.FileStorage;
using System.IO.Abstractions;

namespace Sample.Infrastucture
{
    public static class FileStorageExtensions
    {
        public static IServiceCollection AddLocalFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFileStorage, LocalDiskFileStorage>();
            services.AddTransient<IFileSystem, FileSystem>();
            services.Configure<LocalDiskOptions>(configuration);

            return services;
        }
    }
}
