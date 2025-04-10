namespace DataManagementWithElasticsearch.WebApp.Extensions
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionOptions>
                (configuration.GetSection(ConnectionOptions.ConnectionConfiguration));
            
            services.AddHttpClient();

            services.AddScoped<IElasticsearchService, ElasticsearchService>();
            services.AddScoped<INewsService, NewsService>();

            return services;
        }
    }
}