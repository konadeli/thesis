using System;
using System.Linq;
using Api.Database.Model;
using Api.Services;

namespace Api.Integrations
{
    public class DbIntegration : IDbIntegration
    {
        private readonly IDbService _dbService;
        public DbIntegration(IDbService dbService)
        {
            _dbService = dbService;
        }

        public Aadeuser GetUser(Guid id)
        {
            return _dbService.Set<Aadeuser>().SingleOrDefault(i => i.Id == id);
        }

        public Guid CreateUser(Aadeuser entity)
        {
            var response = _dbService.Create(entity);

            _dbService.Save();

            return response;
        }

        public bool UpdateUser(Aadeuser entity)
        {
            var isUpdated = _dbService.Update(entity.Id, entity);

            _dbService.Save();

            return isUpdated;
        }

        public string GetAadeUserPrivateKey(Guid id)
        {
            return _dbService.Set<Aadeuser>().SingleOrDefault(i => i.Id == id)?.PrivateKey;
        }

        public Message GetMessage(Guid id)
        {
            return _dbService.Set<Message>().SingleOrDefault(i => i.Id == id);
        }
    }
}
