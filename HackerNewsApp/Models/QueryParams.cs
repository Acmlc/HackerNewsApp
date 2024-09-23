using System.ComponentModel.DataAnnotations;

namespace HackerNewsApp.Models
{
    public class QueryParams
    {
        [Required]
        public int PageNumber { get; set; } = 1;

        private int minSize = 10;

        [Required]
        public int PageSize
        {
            get => minSize;
            set => minSize = value > 50 ? 50 : value;
        }
        
        [StringLength(800, MinimumLength = 0)]
        public string? SearchTerm { get; set; }
    }
}
