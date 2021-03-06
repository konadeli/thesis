﻿using System.Collections.Generic;
using System.Linq;
using Users.Models.Aade;
using Users.Services;
using Users.ViewModel;

namespace Users.Integrations
{
    public class AadeDbIntegration : IAadeDbIntegration
    {
        private readonly IAadeDbService _aadeIDbService;
        public AadeDbIntegration(IAadeDbService aadeIDbService)
        {
            _aadeIDbService = aadeIDbService;
        }

        public List<AadeUser> GetAadeUsers()
        {
            var allAadeUsers = _aadeIDbService.Set().ToList();
            var aade = new List<AadeUser>();
            foreach (var aadeUser in allAadeUsers)
            {
                var a = new AadeUser();
                a.AadeUserId = aadeUser.Id;
                a.AadeUserName = aadeUser.UserName;
                a.Email = aadeUser.Email;
                aade.Add(a);
            }

            return aade;
        }

        public string GetAadeUserPublicKey(string id)
        {
            return _aadeIDbService.Set().SingleOrDefault(i => i.Id == id)?.PublicKey;
        }

        public AadeUser GetAadeUserEmailAddress(string id)
        {
            var aadeUser = _aadeIDbService.Set().SingleOrDefault(i => i.Id == id);

            var a = new AadeUser();
            a.AadeUserId = aadeUser.Id;
            a.AadeUserName = aadeUser.UserName;
            a.Email = aadeUser.Email;

            return a;

        }

    }
}
