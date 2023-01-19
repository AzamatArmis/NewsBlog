using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using System.Diagnostics;

namespace NewsBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDbContext _ctx;

        public HomeController(ILogger<HomeController> logger, AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index(SortState sortOrder, 
            string currentFilter, string searchString, 
            int pageNumber = 1, int pageSize = 5)
        {
            IQueryable<NewsModel> models = _ctx.DbNews.AsQueryable();
            List<NewsImageModel> images = _ctx.DbNewsImages.ToList();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = currentFilter;
            ViewData["CurrentPageSize"] = pageSize;
            IndexViewModel viewModel = new IndexViewModel(models, images,
                sortOrder, currentFilter, searchString, 
                pageNumber, pageSize);
            return View(viewModel);
        }

        public IActionResult Privacy() { return View(); }
        public ActionResult ActiveSearch(string title)
        {
            var allNews = _ctx.DbNews.Where(a => a.NewsTitle.Contains(title)).ToList();
            return PartialView(allNews);
        }
        public IActionResult Create() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Create(NewsModel model, 
            IEnumerable<IFormFile> files)
        {
            _ctx.DbNews.Add(model);
            await _ctx.SaveChangesAsync();
            var last = await _ctx.DbNews.OrderBy(p => p.Id).LastOrDefaultAsync();
            var uploadPath = $"C:\\Users\\armis\\Documents\\NewsBlogImages";
            Directory.CreateDirectory(uploadPath);
            foreach (var file in files)
            {
                // путь к папке uploads
                string fullPath = $"{uploadPath}\\{file.FileName}";

                // сохраняем файл в папку uploads
                using (var fileStream = new FileStream(fullPath, 
                    FileMode.Create))
                { await file.CopyToAsync(fileStream); }
                NewsImageModel newsImageModel = new NewsImageModel();
                newsImageModel.Name = file.FileName;
                newsImageModel.NewsId = last.Id;
                last.NewsImages.Add(newsImageModel);
            }
            _ctx.DbNews.Update(last);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult GetFile()
        {
            // Путь к файлу
            string file_path = Path.Combine();
            // Тип файла - content-type
            string file_type = "image/jpeg";
            // Имя файла - необязательно
            string file_name = "hello.txt";
            return PhysicalFile(file_path, file_type, file_name);
        }

        public IActionResult GetImage(string fileName)
        {
            string file_path = $"C:\\Users\\armis\\Documents\\NewsBlogImages\\{fileName}";
            string file_type = "image/jpeg";
            return PhysicalFile(file_path, file_type);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                NewsModel model = new NewsModel { Id = id.Value };
                _ctx.Entry(model).State = EntityState.Deleted;
                await _ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
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
                if (model != null) return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsModel model, IEnumerable<IFormFile> files)
        {
            if (files.Count() != 0)
            {
                string uploadPath = $"C:\\Users\\armis\\Documents\\NewsBlogImages";
                Directory.CreateDirectory(uploadPath);
                foreach (var file in files)
                {
                    // путь к папке uploads
                    string fullPath = $"{uploadPath}\\{file.FileName}";

                    // сохраняем файл в папку uploads
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    { await file.CopyToAsync(fileStream); }
                    NewsImageModel newsImageModel = new NewsImageModel();
                    newsImageModel.Name = file.Name;
                    model.NewsImages.Add(newsImageModel);
                }
            }
            _ctx.DbNews.Update(model);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}