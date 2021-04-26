using System.Linq;
using Aade.Models;
using Aade.Models.Aade;
using Aade.Models.Messages;
using Aade.Services;

namespace Aade.Integrations
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
