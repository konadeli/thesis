using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Aade.Integrations;
using Aade.Models.Aade;
using CryptoExpt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Aade.Controllers
{
    public class DocController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IAadeDbIntegration _aadeDbIntegration;
        private readonly IMessageDbIntegration _messageDbIntegration;

        public DocController(
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
        public IActionResult Index(string docid)
        {
            var id = _userManager.GetUserId(User);
            var user = _aadeDbIntegration.GetUser(id);
            var doc = _messageDbIntegration.GetMessage(docid);

            if (doc == null)
            {
                Response.StatusCode = 400;
                return Content("Document was not found");
            }

            // Grab the AADE user's private key for the decryption of the symmetric key
            // that was used to encrypt the document
            RsaPrivateCrtKeyParameters privateKeyRecovered = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(user.PrivateKey));
            IAsymmetricBlockCipher engine = new RsaEngine();
            engine.Init(false, privateKeyRecovered);

            // the message is saved as a byte array so convert back to Document object
            var message = Document.Deserialize(doc.Message);

            // recover symmetric key
            var derivedKeyBytesToReceive = engine.ProcessBlock(message.EncryptedSymmetricKey, 0, message.EncryptedSymmetricKeyLength);

            // use this now to Decrypt the message 
            var decryptedDocument = AesOperation.DecryptString(Document.ByteArrayToString(derivedKeyBytesToReceive), message.EncryptedDocument);

            var decryptedDocumentAsBytes = Convert.FromBase64String(decryptedDocument);

            // verify signature to ensure message was not tampered with
            var isvalid = VerifySignature(decryptedDocument, doc.UsersPublicKey, doc.Signature);

            doc.Status = 1;
            _messageDbIntegration.UpdateMessage(doc);

            return File(decryptedDocumentAsBytes, doc.ContentType, doc.FileName);
        }


        // Third-party can verify signature against public key to ensure its not been tampered with but cannot
        // change it themselves as they do not have the private key
        private static bool VerifySignature(string message, string publicKey, string signature)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            var publicKeyBytes = Base58Encoding.Decode(publicKey);

            var q = curve.Curve.DecodePoint(publicKeyBytes);

            var keyParameters = new
                ECPublicKeyParameters(q,
                    domain);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

            signer.Init(false, keyParameters);
            signer.BlockUpdate(Encoding.ASCII.GetBytes(message), 0, message.Length);

            var signatureBytes = Base58Encoding.Decode(signature);

            return signer.VerifySignature(signatureBytes);
        }

    }
}