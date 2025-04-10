namespace DataManagementWithElasticsearch.WebApp.Services.Concrete
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly ElasticsearchClient elasticsearchClient;
        private readonly Options.ConnectionOptions connectionOptions;
        private readonly INewsService newsService;
        public ElasticsearchService(IOptions<Options.ConnectionOptions> connectionOptions, INewsService newsService)
        {
            this.connectionOptions = connectionOptions.Value;
            this.newsService = newsService;

            var elasticsearchClientSettings = new ElasticsearchClientSettings(new Uri(this.connectionOptions.Elasticsearch));
            elasticsearchClientSettings.DefaultIndex("news");
            elasticsearchClient = new(elasticsearchClientSettings);
        }

        private async Task<bool> AddNewsAsync(string indexName = "news", CancellationToken cancellationToken = default)
        {
            var newsList = await newsService.GetAllAsync();
            foreach (var news in newsList)
            {
                CreateRequest<News> createRequest = new(news, indexName, news.Id);
                CreateResponse createResponse = await elasticsearchClient.CreateAsync(createRequest, cancellationToken);

                if (!createResponse.IsSuccess()) return false;
            }
            return true;
        }

        private async Task<bool> DeleteNewsAsync(string indexName = "news", CancellationToken cancellationToken = default)
        {
            DeleteIndexResponse deleteIndexResponse = await elasticsearchClient.Indices.DeleteAsync("news", cancellationToken);
            return deleteIndexResponse.IsSuccess();
        }

        public async Task<IReadOnlyCollection<News>> MatchQueryNewsAsync(string field, string queryKeyword, string indexName = "news", CancellationToken cancellationToken = default)
        {
            SearchResponse<News> searchResponse = await elasticsearchClient.SearchAsync<News>(index => index
.Index(indexName).Query(query => query.Match(m => m.Field(field).Query(queryKeyword))), cancellationToken);

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<News>> GetNewsAsync(string indexName = "news", CancellationToken cancellationToken = default)
        {
            var deleteStatus = await DeleteNewsAsync();
            Console.WriteLine("Delete işlemi tamamlandı: " + deleteStatus);
            if (!deleteStatus) return null;

            var addStatus = await AddNewsAsync();
            Console.WriteLine("Add işlemi tamamlandı: " + addStatus);
            if (!addStatus) return null;

            await elasticsearchClient.Indices.RefreshAsync("news");

            SearchResponse<News> searchResponse = await elasticsearchClient.SearchAsync<News>("news", cancellationToken);
            Console.WriteLine("Veri çekildi, toplam kayıt sayısı: " + searchResponse.Documents.Count);
            return searchResponse.Documents;
        }
    }
}