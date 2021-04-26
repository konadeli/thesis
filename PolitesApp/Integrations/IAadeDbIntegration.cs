using System.Collections.Generic;
using Users.ViewModel;

namespace Users.Integrations
{
    public interface IAadeDbIntegration
    {
        public List<AadeUser> GetAadeUsers();

        public string GetAadeUserPublicKey(string email);

        public string GetAadeUserId(string email);
    }
}
