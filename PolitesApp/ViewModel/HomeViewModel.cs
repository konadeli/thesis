using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Users.ViewModel
{
    public class HomeViewModel
    {
        public IFormFile MyDocument { set; get; }
        public bool ShowPasswordEntry { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
        public string AadeUserId { get; set; }
        public List<SelectListItem> AadeUsers { get; set; }
    }
}
