namespace Saned.Delco.Admin.Models
{
    public class UserSearchModel : Pager
    {

        public string Id { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; } 
    }
}