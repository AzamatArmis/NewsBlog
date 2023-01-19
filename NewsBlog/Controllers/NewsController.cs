using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;

namespace NewsBlog.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDbContext _ctx;
        public NewsController(ILogger<HomeController> logger, AppDbContext ctx)
        { _logger = logger; _ctx = ctx; }

        public async Task<IActionResult> Index(SortState sortOrder,
            string currentFilter, string searchString,
            int pageNumber = 1, int pageSize = 5)
        {
            IQueryable<NewsModel> models = _ctx.DbNews.AsQueryable();
            List<NewsImageModel> images = _ctx.DbNewsImages.ToList();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = currentFilter;
            ViewData["CurrentPageSize"] = pageSize;
            IndexViewModel viewModel =
                new IndexViewModel(models, images, sortOrder,
                currentFilter, searchString, pageNumber, pageSize);

            return View(viewModel);
        }
        public async Task<IActionResult> ShowNews(int? id)
        {
            if (id != null)
            {
                NewsModel? model = await _ctx.DbNews.FirstOrDefaultAsync(p => p.Id == id);
                List<NewsImageModel> images = new List<NewsImageModel>();
                foreach (var image in _ctx.DbNewsImages)
                {
                    if (image.NewsId == id)
                    { images.Add(image); }
                }
                model.NewsImages = images;
                int viewCount = (int)model.NewsViewCount; viewCount++;
                model.NewsViewCount = viewCount;
                _ctx.DbNews.Update(model);
                await _ctx.SaveChangesAsync();
                if (model != null) return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> RaiseNewsViewCount(int? id)
        {
            NewsModel? model = await _ctx.DbNews.FirstOrDefaultAsync(p => p.Id == id);
            model.NewsViewCount = model.NewsViewCount++;
            _ctx.DbNews.Update(model);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("ShowNews");
        }
        public IActionResult GetImage(string fileName)
        {
            string file_path = $"C:\\Users\\armis\\Documents\\NewsBlogImages\\{fileName}";
            string file_type = "image/jpeg";
            return PhysicalFile(file_path, file_type);
        }
    }
}

