using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace WebApplication1.Utility
{
    public class Encryption
    {

        public static string Hash(string clearText)
        {
            byte[] clearTextAsBytes = Encoding.UTF32.GetBytes(clearText);
            byte[] digest = Hash(clearTextAsBytes);
            string digestAsString = Convert.ToBase64String(digest);

            return digestAsString;
        }

        public static byte[] Hash(byte[] clearTextBytes)
        {
            SHA512 myAlg = SHA512.Create();
            byte[] digest = myAlg.ComputeHash(clearTextBytes);
            return digest;
        }

        static string password = "Pa$$w0rd";
        static byte[] salt = new byte[] {
            20,1,34,56,78,34,11,111,234,43,180,139,127,34,52,45,255,253,1
        };

        /// <summary>
        /// This method is used to take in clear data and encrypt it and returns back to cipher
        /// </summary>
        /// <param name="clearData"></param>
        /// <returns></returns>
        public static byte[] SymmetricEnryption(byte[] clearData)
        {
            //0. declare the algorithm to use
            Rijndael myAlg = Rijndael.Create();

            //1. first we generate the secret key and iv
            var keys = GenerateKeys();

            //2. load the data into a memoryStream
            MemoryStream msIn = new MemoryStream(clearData);
            msIn.Position = 0; //Making sure that the pointer of the byte to read next is at the beginning so we encrypt everything

            //3. declare where to store the encrypted data
            MemoryStream msOut = new MemoryStream();

            //4. declaring a Stream which handles data encryption
            CryptoStream cs = new CryptoStream(msOut, myAlg.CreateEncryptor(keys.SecretKey, keys.Iv), CryptoStreamMode.Write);

            //5. we start the encrypting engine
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut
            cs.FlushFinalBlock();

            //7
            cs.Close();

            //8
            return msOut.ToArray();
        }

        public static byte[] SymmetricEnryption(byte[] clearData, SymmetricKeys keys)
        {
            //0. declare the algorithm to use
            Rijndael myAlg = Rijndael.Create();

            //1. first we generate the secret key and iv
            //var keys = GenerateKeys();

            //2. load the data into a memoryStream
            MemoryStream msIn = new MemoryStream(clearData);
            msIn.Position = 0; //Making sure that the pointer of the byte to read next is at the beginning so we encrypt everything

            //3. declare where to store the encrypted data
            MemoryStream msOut = new MemoryStream();

            //4. declaring a Stream which handles data encryption
            CryptoStream cs = new CryptoStream(msOut, myAlg.CreateEncryptor(keys.SecretKey, keys.Iv), CryptoStreamMode.Write);

            //5. we start the encrypting engine
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut
            cs.FlushFinalBlock();

            //7
            cs.Close();

            //8
            return msOut.ToArray();
        }

        public static SymmetricKeys GenerateKeys()
        {
            Rijndael myAlg = Rijndael.Create();
            Rfc2898DeriveBytes myGenerator = new Rfc2898DeriveBytes(password, salt);
            SymmetricKeys keys = new SymmetricKeys()
            {
                SecretKey = myGenerator.GetBytes(myAlg.KeySize / 8),
                Iv = myGenerator.GetBytes(myAlg.BlockSize / 8)
            };

            return keys;
        }

        public static byte[] SymmetricDecrypt(byte[] cipherAsBytes)
        {
            //0. declare the algorithm to use
            Rijndael myAlg = Rijndael.Create();

            //1. first we generate the secret key and iv
            var keys = GenerateKeys();

            //2. load the data into a memoryStream
            MemoryStream msIn = new MemoryStream(cipherAsBytes);
            msIn.Position = 0; //Making sure that the pointer of the byte to read next is at the beginning so we encrypt everything

            //3. declare where to store the clear data
            MemoryStream msOut = new MemoryStream();

            //4. declaring a Stream which handles data decryption
            CryptoStream cs = new CryptoStream(msOut, myAlg.CreateDecryptor(keys.SecretKey, keys.Iv), CryptoStreamMode.Write);

            //5. we start the encrypting engine
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut
            cs.FlushFinalBlock();

            //7
            cs.Close();

            //8
            return msOut.ToArray();
        }

        public static byte[] SymmetricDecrypt(byte[] cipherAsBytes, SymmetricKeys keys)
        {
            //0. declare the algorithm to use
            Rijndael myAlg = Rijndael.Create();

            //1. first we generate the secret key and iv

            //2. load the data into a memoryStream
            MemoryStream msIn = new MemoryStream(cipherAsBytes);
            msIn.Position = 0; //Making sure that the pointer of the byte to read next is at the beginning so we encrypt everything

            //3. declare where to store the clear data
            MemoryStream msOut = new MemoryStream();

            //4. declaring a Stream which handles data decryption
            CryptoStream cs = new CryptoStream(msOut, myAlg.CreateDecryptor(keys.SecretKey, keys.Iv), CryptoStreamMode.Write);

            //5. we start the encrypting engine
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut
            cs.FlushFinalBlock();

            //7
            cs.Close();

            //8
            return msOut.ToArray();
        }

        public static string SymmetricEncrypt(string clearData)
        {
            byte[] clearDataAsBytes = Encoding.UTF32.GetBytes(clearData);
            byte[] cipherAsBytes = SymmetricEnryption(clearDataAsBytes);
            string cipher = Convert.ToBase64String(cipherAsBytes);

            cipher = cipher.Replace("/", "£").Replace("+", "_").Replace("=", "*");

            return cipher;
        }

        public static string SymmetricDecrypt(string cipher)
        {
         
                cipher = cipher.Replace("£", "/").Replace("_", "+").Replace("*", "=");



                byte[] clearDataAsBytes = Convert.FromBase64String(cipher);
                byte[] clearAsBytes = SymmetricDecrypt(clearDataAsBytes);

                string originalText = Encoding.UTF32.GetString(clearAsBytes);
                return originalText;
            
         
        }

   

      

        public static AsymmetricKeys GenerateAsymmetricKeys()
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();

            AsymmetricKeys myKeys = new AsymmetricKeys()
            {
                PublicKey = myAlg.ToXmlString(false),
                PrivateKey = myAlg.ToXmlString(true)
            };

            return myKeys;
        }

        public class AsymmetricKeys
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public static byte[] AsymmetricEnrypt(byte[] data, string publicKey)
        {
            /*
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(publicKey);
            byte[] dataAsBytes = Encoding.UTF32.GetBytes(data);
            byte[] cipher = myAlg.Encrypt(dataAsBytes, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(cipher);
            */

            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(publicKey);
            byte[] cipher = myAlg.Encrypt(data, RSAEncryptionPadding.Pkcs1);

            return cipher;
        }

        public static byte[] AsymmetricDecrypt(byte[] cipher, string privateKey)
        {
            /*
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);
            byte[] cipherAsBytes = Convert.FromBase64String(cipher);
            byte[] originalTextAsBytes = myAlg.Decrypt(cipherAsBytes, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF32.GetString(originalTextAsBytes);
            */

            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);
            byte[] originalTextAsBytes = myAlg.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);

            return originalTextAsBytes;
        }

        public static MemoryStream HybridEncrypt(MemoryStream clearFile, string publicKey)
        {
            //1. Generate the symmetric keys
            Rijndael myAlg = Rijndael.Create();
            myAlg.GenerateKey();
            myAlg.GenerateIV();
            var key = myAlg.Key;
            var iv = myAlg.IV;

            //2. Encrypting the clearFile using Symmetric Encryption
            
            SymmetricKeys keys = new SymmetricKeys();
            keys.SecretKey = key;
            keys.Iv = iv;
            

            //3. Asymmetrically encrypt using the public key, the sym key and iv above
            //string keyS = Convert.ToBase64String(key);
            byte[] encryptedKey = AsymmetricEnrypt(key, publicKey);

            //4. Store the above encrypted data n one file
            byte[] encryptedIv = AsymmetricEnrypt(iv, publicKey);

            MemoryStream msOut = new MemoryStream();
            msOut.Write(encryptedKey, 0, encryptedKey.Length);
            msOut.Write(encryptedIv, 0, encryptedIv.Length);

            byte[] bytes = clearFile.ToArray();
            byte[] encryptedBytes = SymmetricEnryption(bytes, keys);

            MemoryStream encryptedfileContent = new MemoryStream(encryptedBytes);
            encryptedfileContent.Position = 0;
            encryptedfileContent.CopyTo(msOut);
            
            return msOut;
        }

        public static MemoryStream HybridDecrypt(MemoryStream encFile, string privateKey)
        {
            encFile.Position = 0;

            //1) Read enc key
            byte[] retrievedEncKey = new byte[128];
            encFile.Read(retrievedEncKey,0,128);

            //2) Read enc iv
            byte[] retrievedEncIv = new byte[128];
            encFile.Read(retrievedEncIv, 0, 128);

            //3) Decrypt using the privatekey (asymmetric) the 1 and 2
            byte[] decKey = AsymmetricDecrypt(retrievedEncKey, privateKey);
            byte[] decIv = AsymmetricDecrypt(retrievedEncIv, privateKey);
            
            SymmetricKeys keys = new SymmetricKeys();
            keys.SecretKey = decKey;
            keys.Iv = decIv;
            /*
            MemoryStream msOut = new MemoryStream();
            msOut.Write(decKey, 0, decKey.Length);
            msOut.Write(decIv, 0, decIv.Length);
            */
            //4) Read the rest of the file (which is the actual file content)

            MemoryStream actualencryptedfilecontent = new MemoryStream();
            encFile.CopyTo(actualencryptedfilecontent);

            //5 Symmetrically dec what you read in no 4 using what you dec in step no 3
            byte[] bytes = actualencryptedfilecontent.ToArray();
            byte[] decFile = SymmetricDecrypt(bytes, keys);

            MemoryStream actualFile = new MemoryStream(decFile);
            return actualFile;
        }

        public static string SignData(MemoryStream data, string privateKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);

            //Change the data from MemoryStream into byte[]
            byte[] dataAsBytes = data.ToArray();

            //Hash the data
            byte[] digest = Hash(dataAsBytes);
            byte[] signatureAsBytes = myAlg.SignHash(digest, "SHA512");

            //save the signature in the database > table containing the file data

            return Convert.ToBase64String(signatureAsBytes);
        }

        public static bool VerifyData(MemoryStream data, string publicKey, string signature)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(publicKey);

            byte[] dataAsBytes = data.ToArray();
            byte[] digest = Hash(dataAsBytes);

            byte[] signatureAsBytes = Convert.FromBase64String(signature);
            bool valid = myAlg.VerifyHash(digest, "SHA512", signatureAsBytes);
            return valid;
        }

        public class SymmetricKeys
        {
            public byte[] SecretKey { get; set; }
            public byte[] Iv { get; set; }
        }
    }
}
