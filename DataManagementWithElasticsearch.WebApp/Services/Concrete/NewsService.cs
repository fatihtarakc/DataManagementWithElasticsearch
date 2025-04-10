namespace DataManagementWithElasticsearch.WebApp.Services.Concrete
{
    public class NewsService : INewsService
    {
        public async Task<List<News>> GetAllAsync()
        {
            string url = "https://www.sozcu.com.tr";
            try
            {
                var client = new HttpClient();
                string htmlContent = await client.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.OptionFixNestedTags = true;
                htmlDocument.LoadHtml(htmlContent);

                var newsNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='container position-relative mb-4']/div[@class='row']/div[@class='col-lg-12 mb-0 mb-lg-4 surmanset order-3 order-lg-1']/div[@class='row']/div[@class='col-md-6 col-lg-3 mb-4 mb-lg-0']");

                if (newsNodes is null)
                {
                    Console.WriteLine("Cannot be found news !");
                    return new List<News>();
                }

                var newsList = new List<News>();

                foreach (HtmlNode newsNode in newsNodes)
                {
                    var newsCard = newsNode.SelectSingleNode(".//div[@class='news-card']");
                    var newsCardImg = newsCard.SelectSingleNode(".//a[@class='img-holder wide mb-2']/img");
                    var newsCardFooter = newsCard.SelectSingleNode(".//a[@class='news-card-footer']");

                    var news = new News();
                    news.Id = Guid.NewGuid();
                    news.Image = newsCardImg.GetAttributeValue("src", string.Empty).Trim();
                    news.Link = $"{url}" + newsCardFooter.GetAttributeValue("href", string.Empty).Trim();
                    var content = newsCardFooter.InnerText.Trim();
                    news.Content = WebUtility.HtmlDecode(content).Trim('\'');

                    newsList.Add(news);

                    Console.WriteLine($"Id: ${news.Id}");
                    Console.WriteLine($"Link: ${news.Image}");
                    Console.WriteLine($"Link: ${news.Link}");
                    Console.WriteLine($"Content: ${news.Content}");
                    Console.WriteLine("----------------------------------------------------");
                }
                return newsList;
            }
            catch
            {
                Console.WriteLine("An error occurred while fetching the data !");
                return new List<News>();
            }
        }
    }
}