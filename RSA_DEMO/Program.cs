using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace RSA_DEMO
{
    class RSA_DEMO
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RSA Demo");

            //data to encrypt
            string plainTextData = "Hello World! ISM6225";
            Console.WriteLine("Text to encrypt: " + plainTextData);

            //lets take a new CSP with a new 2048 bit rsa key pair
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);            
            RSAParameters privKey = csp.ExportParameters(true);                     
            RSAParameters pubKey = csp.ExportParameters(false);
            Console.WriteLine("\nPublic key is:\n" + getKeyString(pubKey) + "\n");
            Console.WriteLine("Private key is:\n" + getKeyString(privKey) + "\n");

            //encrypt            
            string cypherText = encrypt(plainTextData, pubKey);
            Console.WriteLine("Cypher text: " + cypherText);

            //decrypt back
            string originalText = decrypt(cypherText, privKey);
            Console.WriteLine("\nDecrypted text: " + originalText);
                                   
        }

        //Serialize RSA key into XML
        public static string getKeyString(RSAParameters pubKey)
        {
            StringWriter sw = new System.IO.StringWriter();
            //serialize data into StringWriter
            XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, pubKey);
            //Convert to string
            return sw.ToString();
        }


        public static string encrypt(string str, RSAParameters pubKey)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);
            byte[] bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(str);
            //pkcs#1.5 padding 
            byte[] bytesCypherText = csp.Encrypt(bytesPlainTextData, false);
            return Convert.ToBase64String(bytesCypherText);
        }

        public static string decrypt(string cypherText, RSAParameters privKey)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);
            byte[] bytesCypherText = Convert.FromBase64String(cypherText);
            //pkcs#1.5 padding 
            byte[] decryptedTextBytes = csp.Decrypt(bytesCypherText, false);
            return System.Text.Encoding.Unicode.GetString(decryptedTextBytes);
        }
    }
}
