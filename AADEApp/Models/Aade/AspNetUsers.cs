using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Aade.Services;

namespace Aade.Models.Aade
{
    public partial class AspNetUsers: IdentityUser, IEntity
    {
        public AspNetUsers()
        {
        }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

    }
}
