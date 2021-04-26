using System.Collections.Generic;
using Aade.ViewModel;

namespace Aade.Integrations
{
    public interface IAadeDbIntegration
    {
        public List<AadeUser> GetAadeUsers();

        public string GetAadeUserPublicKey(string email);

        public string GetAadeUserId(string email);
    }
}
