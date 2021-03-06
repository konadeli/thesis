﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CryptoExpt
{
    [Serializable]
    public class Document
    {
        public string FromPublicKey { get; set; }
        public int EncryptedSymmetricKeyLength { get; set; }
        public byte[] EncryptedSymmetricKey { get; set; }
        public string EncryptedDocument { get; set; }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(FromPublicKey);
                    writer.Write(EncryptedSymmetricKeyLength);
                    writer.Write(EncryptedSymmetricKey);
                    writer.Write(EncryptedDocument);
                }
                return m.ToArray();
            }
        }

        public static Document Deserialize(byte[] data)
        {
            Document result = new Document();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.FromPublicKey = reader.ReadString();
                    result.EncryptedSymmetricKeyLength = reader.ReadInt32();
                    result.EncryptedSymmetricKey = reader.ReadBytes(result.EncryptedSymmetricKeyLength);
                    result.EncryptedDocument = reader.ReadString();
                }
            }
            return result;
        }

    }
}
