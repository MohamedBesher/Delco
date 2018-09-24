namespace Saned.Delco.Admin.Models
{
    public class Pager
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Keyword { get; set; } = string.Empty;
        public long Id { get; set; }
        public string UserId { get; set; }


    }
}