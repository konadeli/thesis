using System.Linq;
using Users.Models;
using Users.Models.Aade;
using Users.Models.Messages;
using Users.Services;

namespace Users.Integrations
{
    public class MessageDbIntegration : IMessageDbIntegration
    {
        private readonly IMessagesDbService _messageIDbService;
        public MessageDbIntegration(IMessagesDbService messageIDbService)
        {
            _messageIDbService = messageIDbService;
        }

        public string CreateMessage(Messages entity)
        {
            var response = _messageIDbService.Create(entity);

            _messageIDbService.Save();

            return response;
        }

    }
}
