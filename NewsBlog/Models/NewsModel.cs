namespace NewsBlog.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public int? NewsViewCount { get; set; } = 0;
        public DateTime NewsLastModified { get; set; } = DateTime.Now;
        //public string? NewsImagePath { get; set; }
        public List<NewsImageModel> NewsImages { get; set; } = new();
        public string? NewsTitle { get; set; }
        public string? NewsDescription { get; set; }
        public string? NewsContent { get; set; }
    }
}
