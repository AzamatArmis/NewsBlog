namespace NewsBlog.Models
{
    public class NewsImageModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NewsId { get; set; }
        public NewsModel? NewsModel { get; set; }
    }
}
