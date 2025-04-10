namespace DataManagementWithElasticsearch.WebApp.Services.Abstract
{
    public interface INewsService
    {
        Task<List<News>> GetAllAsync();
    }
}