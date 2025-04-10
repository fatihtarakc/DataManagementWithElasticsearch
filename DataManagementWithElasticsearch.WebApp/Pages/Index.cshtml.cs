namespace DataManagementWithElasticsearch.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly IElasticsearchService elasticsearchService;
    public IndexModel(IElasticsearchService elasticsearchService)
    {
        this.elasticsearchService = elasticsearchService;

        NewsList = new List<News>();
    }

    [BindProperty]
    public string SearchQuery { get; set; }
    public List<News> NewsList { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        NewsList = (await elasticsearchService.GetNewsAsync()).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
            NewsList = (await elasticsearchService.GetNewsAsync()).ToList();
        else NewsList = (await elasticsearchService.MatchQueryNewsAsync("content", SearchQuery)).ToList();

        return Page();
    }
}