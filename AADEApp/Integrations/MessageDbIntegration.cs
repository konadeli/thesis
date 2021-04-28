using System.Collections.Generic;
using System.Linq;
using Aade.Services;
using Aade.ViewModel;

namespace Aade.Integrations
{
    public class MessageDbIntegration : IMessageDbIntegration
    {
        private readonly IMessagesDbService _messageIDbService;
        private readonly IUserDbIntegration _userDbIntegration;

        public MessageDbIntegration(IMessagesDbService messageIDbService, IUserDbIntegration userDbIntegration)
        {
            _messageIDbService = messageIDbService;
            _userDbIntegration = userDbIntegration;
        }

        public List<MessagesForMe> GetMessageForAadeUser(string id)
        {
            var messages = _messageIDbService.Set()
                .Where(a => a.AadeuserId == id).ToList();
            var myMessages = new List<MessagesForMe>();
            foreach (var message in messages)
            {
                var p = _userDbIntegration.GetUser(message.PolitisUserId);
                var m = new MessagesForMe();
                m.ContentType = message.ContentType;
                m.DateCreated = message.DateCreated;
                m.DateModified = message.DateModified;
                m.FileName = message.FileName;
                m.Id = message.Id;
                m.PolitisUserId = p.Id;
                m.PolitisEmail = p.Email;
                m.PolitisName = p.UserName;
                m.Status = message.Status;
            }
            return myMessages;
        }

    }
}
