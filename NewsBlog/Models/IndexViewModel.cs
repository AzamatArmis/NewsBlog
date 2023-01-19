using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsBlog.Models
{
    public class IndexViewModel
    {
        public IQueryable<NewsModel> NewsModels { get; set; }
        public List<NewsImageModel> NewsImagesList { get; set; }
        public int[] Numbers { get; set; } = new int[] { 1, 3, 5, 10, 15, 20 };
        public IEnumerable<int> Sizes { get; set; }
        public SelectList PageSizes { get; set; }
        public SortState CurrentSort { get; set; }
        public Dictionary<string, SortState> SortModes { get; set; } = 
            new Dictionary<string, SortState>()
            {
                { "возростанию заголовка", SortState.NewsTitleAsc },
                { "убыванию заголовка", SortState.NewsTitleDesc },
                { "возростанию описания", SortState.NewsDescriptionAsc },
                { "убыванию описания", SortState.NewsDescriptionDesc },
                { "возростанию даты", SortState.NewsLastModifiedAsc },
                { "убыванию даты", SortState.NewsLastModifiedDesc }
            };
        public SelectList SelectSorts { get; set; }
        public string CurrentFilter { get; set; }
        public int CurrentPageSize { get; set; } = 3;
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public IndexViewModel(IQueryable<NewsModel> newsModels, 
            List<NewsImageModel> images, SortState sortOrder, 
            string currentFilter, string searchString, 
            int pageNumber, int pageSize)
        {
            NewsModels = newsModels.AsQueryable();
            NewsImagesList = images.ToList();
            Sizes = Numbers;
            PageSizes = new SelectList(Sizes);
            SelectSorts = new SelectList(SortModes,"Value", "Key");
            CurrentSort = sortOrder;
            CurrentFilter = currentFilter;
            CurrentPageSize = pageSize;
            PageIndex = pageNumber;
            TotalPages = (int)Math.Ceiling((NewsModels.Count()) / (double)pageSize);
            if (searchString != null)
            { pageNumber = 1; }
            else
            { searchString = currentFilter; }

            if (!String.IsNullOrEmpty(CurrentFilter))
            { NewsModels = NewsModels.Where(m => m.NewsTitle.Contains(CurrentFilter)); }

            NewsModels = sortOrder switch
            {
                SortState.NewsTitleDesc => NewsModels.OrderByDescending(m => m.NewsTitle),
                SortState.NewsLastModifiedAsc => NewsModels.OrderBy(m => m.NewsLastModified),
                SortState.NewsLastModifiedDesc => NewsModels.OrderByDescending(m => m.NewsLastModified),
                SortState.NewsDescriptionAsc => NewsModels.OrderBy(m => m.NewsDescription),
                SortState.NewsDescriptionDesc => NewsModels.OrderByDescending(m => m.NewsDescription),
                _ => NewsModels.OrderBy(m => m.NewsTitle),
            };

            NewsModels = NewsModels.Skip((PageIndex - 1) * CurrentPageSize).Take(CurrentPageSize);
        }
    }
}
