using HistoricalMonumentsWebApplication.Models.Entities;

namespace HistoricalMonumentsWebApplication.Models
{
    public class PaginationModel
    {
        public List<HistoricalMonument> HistoricalMonuments { get; set; }

        public int? Pages { get; set; }

        public int? CurrentPage { get; set;}

        public string Category { get; set; }
        public string? CategoryOption { get; set; }
    }
}
