using System;
using System.Collections.Generic;
using Aade.Models;
using Aade.Models.Messages;
using Aade.ViewModel;

namespace Aade.Integrations
{
    public interface IMessageDbIntegration
    {
        public List<MessagesForMe> GetMessageForAadeUser(string id);
    }
}
