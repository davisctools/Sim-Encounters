using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class XmlDocumentDeserializer : IStringDeserializer<XmlDocument>
    {
        protected virtual string EncryptionKey { get; } = "obexOpm1wWM7NGPV";
        protected virtual string EncryptionIV { get; } = "fTfB28G5j3Pmsw1p";

        public virtual XmlDocument Deserialize(string text)
        {
            if (text == null || text.Equals("")) {
                Debug.Log("Nothing to load!");
                return null;
            }

            var xmlDoc = new XmlDocument();
            try {
                xmlDoc.LoadXml(text);
            } catch (XmlException) {
                text = DecryptXml(text);
                xmlDoc.LoadXml(text);
            }

            return xmlDoc;
        }

        protected virtual string DecryptXml(string text)
        {
            AesManaged aes = new AesManaged();
            ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(EncryptionKey), Encoding.UTF8.GetBytes(EncryptionIV));
            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            using (MemoryStream ms = new MemoryStream(byteArray))
            using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs)) {
                return sr.ReadToEnd();
            }
        }
    }
}