using System;
using Users.Models;
using Users.Models.Messages;

namespace Users.Integrations
{
    public interface IMessageDbIntegration 
    {
        public string CreateMessage(Messages entity);
    }
}
