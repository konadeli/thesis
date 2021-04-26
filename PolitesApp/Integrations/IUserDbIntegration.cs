
using Users.Models.User;

namespace Users.Integrations
{
    public interface IUserDbIntegration 
    {
        public AspNetUsers GetUser(string id);
        public bool UpdateUser(AspNetUsers entity);
    }
}
