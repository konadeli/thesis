using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Users.ViewModel
{
    public class AadeUser
    {
        [BindProperty]
        public string AadeUserId { get; set; }
        public string AadeUserName { get; set; }
        public string Email { get; set; }
    }
}
