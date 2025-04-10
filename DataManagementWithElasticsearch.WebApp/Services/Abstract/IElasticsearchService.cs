namespace DataManagementWithElasticsearch.WebApp.Services.Abstract
{
    public interface IElasticsearchService
    {
        Task<IReadOnlyCollection<News>> GetNewsAsync(string indexName = "News", CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<News>> MatchQueryNewsAsync(string field, string queryKeyword, string indexName = "news", CancellationToken cancellationToken = default);
    }
}