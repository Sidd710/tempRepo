using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Atum.Studio.Core.Managers
{
    public class LicenseManager
    {
        public void Test(string privateKey)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                MessageBox.Show("Create or open a private/public key pair file");
                return;
            }
            StringBuilder licenseContent = new StringBuilder();
            licenseContent.Append("<license>"); licenseContent.AppendFormat("<customer>{0}</customer>", "Customer");
            licenseContent.AppendFormat("<company>{0}</company>", "Company");
            licenseContent.AppendFormat("<trial>{0}</trial>", "Days");
            licenseContent.Append("</license>");

            LicenseFile license = new LicenseFile();

            XmlDocument fileContent = license.SignXmlDocument(licenseContent.ToString(), privateKey); StringToFile(@"c:\temp\testlicense.lic", fileContent.OuterXml);

        }

        public string LoadKeyPairs(string file)
        {
            StreamReader stream = System.IO.File.OpenText(file);
            return stream.ReadToEnd();
        }
        public void CreateKeyPairs(string directory)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(2048);

            string publicPrivateKeyXML = key.ToXmlString(true);
            string publicOnlyKeyXML = key.ToXmlString(false);

            StringToFile(directory + "_public.xml", publicOnlyKeyXML);
            StringToFile(directory + "_private.xml", publicPrivateKeyXML);
        }
        private void StringToFile(string outfile, string data)
        {
            StreamWriter outStream = System.IO.File.CreateText(outfile);
            outStream.Write(data);
            outStream.Close();
        }

    }
}

    public class LicenseFile
    {
        public bool VerifyXmlDocument(string publicKey, string licenseContent)
        {

            RSA key = RSA.Create();
            key.FromXmlString(publicKey);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(licenseContent);
            SignedXml sxml = new SignedXml(doc);
            try
            {
                // Find signature node
                XmlNode sig = doc.GetElementsByTagName("Signature")[0];
                sxml.LoadXml((XmlElement)sig);
            }
            catch (Exception ex)
            {
                // Not signed!
                return false;
            }
            return sxml.CheckSignature(key);
        }
        public XmlDocument SignXmlDocument(string licenseContent, string privateKey)
        {
            RSA key = RSA.Create();
            key.FromXmlString(privateKey);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(licenseContent);

            SignedXml sxml = new SignedXml(doc);
            sxml.SigningKey = key;
            sxml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;

            // Add reference to XML data
            Reference r = new Reference("");
            r.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
            sxml.AddReference(r);

            // Build signature
            sxml.ComputeSignature();

            // Attach signature to XML Document
            XmlElement sig = sxml.GetXml();
            doc.DocumentElement.AppendChild(sig);

            return doc;
        }
    }

