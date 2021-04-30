using System;
using System.Collections.Generic;
using Aade.Models;
using Aade.Models.Messages;
using Aade.Services;
using Aade.ViewModel;

namespace Aade.Integrations
{
    public interface IMessageDbIntegration
    {
        public List<MessagesForMe> GetMessageForAadeUser(string id);

        public Messages GetMessage(string id);

        public bool UpdateMessage(Messages entity);

    }
}
