using System.Collections.Generic;
using System.Linq;
using Aade.Models.Aade;
using Aade.Services;
using Aade.ViewModel;

namespace Aade.Integrations
{
    public class AadeDbIntegration : IAadeDbIntegration
    {
        private readonly IAadeDbService _aadeIDbService;
        public AadeDbIntegration(IAadeDbService aadeIDbService)
        {
            _aadeIDbService = aadeIDbService;
        }

        public AspNetUsers GetUser(string id)
        {
            return _aadeIDbService.Set().SingleOrDefault(i => i.Id == id);
        }

        public string GetAadeUserPublicKey(string email)
        {
            return _aadeIDbService.Set().SingleOrDefault(i => i.Email == email)?.PublicKey;
        }

        public string GetAadeUserId(string email)
        {
            return _aadeIDbService.Set().SingleOrDefault(i => i.Email == email)?.Id;
        }


        public bool UpdateUser(AspNetUsers entity)
        {
            var isUpdated = _aadeIDbService.Update(entity.Id, entity);

            _aadeIDbService.Save();

            return isUpdated;
        }


    }
}
