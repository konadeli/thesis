﻿
using Aade.Models.User;

namespace Aade.Integrations
{
    public interface IUserDbIntegration 
    {
        public AspNetUsers GetUser(string id);
    }
}
