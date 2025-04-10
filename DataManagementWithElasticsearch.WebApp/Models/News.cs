namespace DataManagementWithElasticsearch.WebApp.Models
{
    public class News
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
    }
}