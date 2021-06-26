using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Aade.Integrations;
using ErrorViewModel = Aade.Models.ErrorViewModel;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Aade.Models.Aade;
using Aade.ViewModel;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace Aade.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IAadeDbIntegration _aadeDbIntegration;
        private readonly IMessageDbIntegration _messageDbIntegration;


        public HomeController(
            UserManager<AspNetUsers> userManager,
            IAadeDbIntegration aadeDbIntegration,
            IMessageDbIntegration messageDbIntegration,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _aadeDbIntegration = aadeDbIntegration;
            _messageDbIntegration = messageDbIntegration;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var id = _userManager.GetUserId(User);
            var user = _aadeDbIntegration.GetUser(id);
            // first time there is no public/private key pair for encryption
            // so generate now
            // generated keypair using RSA that is unique to this aade user
            // https://cryptobook.nakov.com/asymmetric-key-ciphers/the-rsa-cryptosystem-concepts
            if (string.IsNullOrEmpty(user.PrivateKey))
            {
                var keyGenerator = new RsaKeyPairGenerator();
                var param = new KeyGenerationParameters(
                    new SecureRandom(),
                    4096);
                keyGenerator.Init(param);
                var keyPair = keyGenerator.GenerateKeyPair();

                // need to save these keys in the db so must serialize into strings
                // https://stackoverflow.com/questions/22008337/generating-keypair-using-bouncy-castle
                PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);

                var aadeprivateKey = Convert.ToBase64String(pkInfo.GetDerEncoded());
                var aadepublicKey = Convert.ToBase64String(info.GetDerEncoded());

                user.PrivateKey = aadeprivateKey;
                user.PublicKey = aadepublicKey;
                _aadeDbIntegration.UpdateUser(user);
            }

            var model = new HomeViewModel();
            var m = _messageDbIntegration.GetMessageForAadeUser(id);
            model.MyMessages = m;
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
