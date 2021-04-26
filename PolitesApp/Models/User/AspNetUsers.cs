using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Users.Services;

namespace Users.Models.User
{
    public partial class AspNetUsers : IdentityUser, IEntity
    {
        public AspNetUsers()
        {
        }

        public string PublicKey { get; set; }
        public string Salt { get; set; }
        public string SigningPassword { get; set; }

    }
}
