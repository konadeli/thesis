using System.Collections.Generic;
using System.Linq;
using Aade.Models.Messages;
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

        public bool UpdateMessage(Messages entity)
        {
            var response = _messageIDbService.Update(entity.Id, entity);

            _messageIDbService.Save();

            return response;
        }

        public Messages GetMessage(string id)
        {
            return _messageIDbService.Set()
                .FirstOrDefault(a => a.Id == id);
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
                m.DateModified = message.DateModified.ToShortDateString();
                m.FileName = message.FileName;
                m.Id = message.Id;
                m.PolitisUserId = p.Id;
                m.PolitisEmail = p.Email;
                m.PolitisName = p.UserName;
                m.Status = message.Status == 0 ? "Unread" : "Read";

                myMessages.Add(m);
            }
            return myMessages;
        }

    }
}
