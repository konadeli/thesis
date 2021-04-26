using System;
using System.IO;

namespace CryptoExpt
{
    [Serializable]
    public class Document
    {
        public string FromPublicKey { get;  set; }
        public int Length { get; set; }
        public byte[] FileAsByteArray { get;  set; }
        public string Signature { get;  set; }


        public override string ToString()
        {
            return $"{ByteArrayToString(FileAsByteArray)}:{FromPublicKey}";
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public byte[] Serialize()
        {
            using var m = new MemoryStream();
            using (var writer = new BinaryWriter(m))
            {
                writer.Write(FromPublicKey);
                writer.Write(Length);
                writer.Write(FileAsByteArray);
                writer.Write(Signature);
            }
            return m.ToArray();
        }

    }
}
