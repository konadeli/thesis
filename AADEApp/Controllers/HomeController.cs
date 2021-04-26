using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CryptoExpt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Aade.Integrations;
using Aade.ViewModel;
using ErrorViewModel = Aade.Models.ErrorViewModel;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Aade.Models.Messages;
using Aade.Models.User;

namespace Aade.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IUserDbIntegration _userDbIntegration;
        private readonly IAadeDbIntegration _aadeDbIntegration;
        private readonly IMessageDbIntegration _messageDbIntegration;


        public HomeController(
            UserManager<AspNetUsers> userManager,
            IUserDbIntegration userDbIntegration,
            IAadeDbIntegration aadeDbIntegration,
            IMessageDbIntegration messageDbIntegration,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _userDbIntegration = userDbIntegration;
            _aadeDbIntegration = aadeDbIntegration;
            _messageDbIntegration = messageDbIntegration;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var id = _userManager.GetUserId(User);
            var user = _userDbIntegration.GetUser(id);
            // first time there is no SALT so generate a random one which is
            // used every time afterwards

            if (string.IsNullOrEmpty(user.Salt))
            {
                var saltString = Guid.NewGuid();
                user.Salt = saltString.ToString();
                _userDbIntegration.UpdateUser(user);
            }

            var aadeUserList = _aadeDbIntegration.GetAadeUsers();

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(HomeViewModel model)
        {
            var id = _userManager.GetUserId(User);
            var user = _userDbIntegration.GetUser(id);

            if (model.MyDocument != null)
            {

                if (model.MyDocument.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.MyDocument.CopyTo(ms);
                        var array = ms.ToArray();
                        var salt = Encoding.ASCII.GetBytes(user.Salt);
                        var bytes = KeyDerivation.Pbkdf2(model.Password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);
                        var derivedKey = ByteArrayToString(bytes);
                        Console.WriteLine("Derived Key: " + derivedKey + Environment.NewLine);

                        // sign the message
                        var publicKey = GetPublicKeyFromPrivateKeyEx(bytes);
                        var message = new Document { FromPublicKey = publicKey, Length = array.Length, FileAsByteArray = array };
                        message.Signature = GetSignature(bytes, message.ToString());

                        Console.WriteLine("Signature: " + message.Signature + Environment.NewLine);
                        Console.WriteLine("Signature public Key: " + publicKey + Environment.NewLine);


                        // next the asymmetric encryption using AADE users public key
                        // after this, only the selected aade user can decrypt the message...
                        // first get the public key which is saved in a serialized form so must be converted 
                        var aaudeUserPublicKey = _aadeDbIntegration.GetAadeUserPublicKey(model.AadeEmail);
                        RsaKeyParameters publicKeyRecovered = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(aaudeUserPublicKey));
                        // now encrypt
                        IAsymmetricBlockCipher engine = new RsaEngine();
                        engine.Init(true, publicKeyRecovered);
                        var blob = message.Serialize();
                        Console.WriteLine("Sending before encryption:" + Convert.ToBase64String(blob) + Environment.NewLine);
                        var msgToShare = engine.ProcessBlock(blob, 0, blob.Length);
                        Console.WriteLine("Sending after encryption:" + Convert.ToBase64String(msgToShare) + Environment.NewLine);

                        // save to DB
                        var messageToSave = new Messages();
                        messageToSave.Message = msgToShare;
                        messageToSave.AadeuserId = _aadeDbIntegration.GetAadeUserId(model.AadeEmail);
                        messageToSave.DateCreated = DateTime.UtcNow;
                        messageToSave.DateModified = DateTime.UtcNow;
                        messageToSave.PolitisUserId = id;
                        messageToSave.Status = 0;
                        messageToSave.FileName = Path.GetFileName(model.MyDocument.FileName);
                        messageToSave.ContentType = model.MyDocument.ContentType;

                        _messageDbIntegration.CreateMessage(messageToSave);

                        // notify AADE user by email


                    }
                }
            }
            // to do  : Return something
            return RedirectToAction("Index", "Home");
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

        private static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        private static string GetPublicKeyFromPrivateKeyEx(byte[] privateKey)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            var d = new Org.BouncyCastle.Math.BigInteger(privateKey);
            var q = domain.G.Multiply(d);

            var publicKey = new ECPublicKeyParameters(q, domain);
            return Base58Encoding.Encode(publicKey.Q.GetEncoded());
        }

        private static string GetSignature(byte[] privateKey, string message)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            var keyParameters = new
                ECPrivateKeyParameters(new Org.BouncyCastle.Math.BigInteger(privateKey),
                    domain);

            var signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(true, keyParameters);
            signer.BlockUpdate(Encoding.ASCII.GetBytes(message), 0, message.Length);
            var signature = signer.GenerateSignature();
            return Base58Encoding.Encode(signature);
        }
    }
}
