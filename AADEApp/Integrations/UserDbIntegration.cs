using System.Linq;
using Aade.Models.User;
using Aade.Services;

namespace Aade.Integrations
{
    public class UserDbIntegration : IUserDbIntegration
    {
        private readonly IUserDbService _userIDbService;
        public UserDbIntegration(IUserDbService userIDbService)
        {
            _userIDbService = userIDbService;
        }

        public AspNetUsers GetUser(string id)
        {
            return _userIDbService.Set().SingleOrDefault(i => i.Id == id);
        }

    }
}
