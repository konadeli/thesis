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
                a.AadeUserName = aadeUser.NormalizedUserName;
                a.Email = aadeUser.NormalizedEmail;
                aade.Add(a);
            }

            return aade;
        }

        public string GetAadeUserPublicKey(string email)
        {
            return _aadeIDbService.Set().SingleOrDefault(i => i.Email == email)?.PublicKey;
        }

        public string GetAadeUserId(string email)
        {
            return _aadeIDbService.Set().SingleOrDefault(i => i.Email == email)?.Id;
        }


    }
}