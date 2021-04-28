using System.Collections.Generic;
using Aade.Models.Aade;
using Aade.ViewModel;

namespace Aade.Integrations
{
    public interface IAadeDbIntegration
    {
        public AspNetUsers GetUser(string id);

        public string GetAadeUserPublicKey(string email);

        public string GetAadeUserId(string email);

        public bool UpdateUser(AspNetUsers entity);
    }
}
