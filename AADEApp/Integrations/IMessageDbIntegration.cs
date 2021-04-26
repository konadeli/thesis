using System;
using Aade.Models;
using Aade.Models.Messages;

namespace Aade.Integrations
{
    public interface IMessageDbIntegration 
    {
        public string CreateMessage(Messages entity);
    }
}
