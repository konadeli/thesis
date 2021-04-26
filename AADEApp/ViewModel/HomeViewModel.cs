using Microsoft.AspNetCore.Http;

namespace Aade.ViewModel
{
    public class HomeViewModel
    {
        public IFormFile MyDocument { set; get; }
        public string Password { get; set; }
        public string AadeEmail { get; set; }
    }
}
