using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace WebApplication1.Utility
{
    //Symmetric encrpytion/decryption
    //Same key to encryption and decrypt
    //Adv: Fastest, simpler
    //it uses Secret Key and IV + salt
    //Disadv: you have to be very careful if you are going to use this one when sending files over an inescure network
    //Asymmetric encryption/decryption
    //it uses to different keys: Public Key and Private Key
    //Public Key : Encrypt
    //Private Key : decrypt (its always recommended to send over an insecure network the private key)
    //Adv : we now can transfer the keys securely over an insecure network
    //Disadv : This is a slow performing algorithm, you cannot encrypt/decrypt large data. normally used to encrypt base64 data

    //Hybrid Encryption/decryption  //this is needed to encrypt the files uploaded by the student & decrypt the files when they are needed by the teachers
    //we r going to take the adv of both the symmetic and the asymmetric to from the hybrid
    //we are going to send over the insecure network the encrypted data which was encrypted symmetrically and therefore we are going to send the
    //encrypted symmetric keys (which were encrypted with the public key)
    //Hashing
    //1 way encryption
    //usage: in passwords
    //sha 512
    //two different values should give two different digests e.g. "hello world" and "hello world" >> 2 different digests

    //Digital Signing
    //against repudiation (when an attacker tampers with the data and he denies the activity)
    //suggestion for the assignment : you generate a private & public key for every student
    //when the user uploads the file, you take the private key and you sign the file and you store the signature with the file's data
    //when the user/teacher needs to download/view the file we need to use the file's owner public key + signature, to verify the data is still same
    //i.e belonging to that user

    public class Encryption
    {
        /// This method takes a string and returns the digest as a string
        /// 
        public class SymmetricKeys
        {
            public byte[] SecretKey { get; set; }
            public byte[] Iv { get; set; }
        }

        public string Hash(string clearText)
        {
            //convert the clearText into an array of bytes
            //cleartext is a non base64 data it has to be converted this way:
            byte[] clearTextAsBytes = Encoding.UTF32.GetBytes(clearText);
            byte[] digest = Hash(clearTextAsBytes);
            //base64 data convert to/from  a string we have to use this class:
            string digestAsStrings = Convert.ToBase64String(digest);

            return digestAsStrings;
        }

        public byte[] Hash(byte[] clearTextBytes)
        {
            SHA512 myAlg = SHA512.Create();

            byte[] digest = myAlg.ComputeHash(clearTextBytes);
            return digest;

        }

        string password = "Pa$$w0rd";

        byte[] salt = new byte[]
        {
            20,1,34,56,78,34,11,111,234,43,180,139,127,34,52,45,255,253,1
        };

        //this method is used to take in clear data and encrypt it and returns back the cipher (the encrypted data)

        public byte[] SymmetricEncrypt(byte[] clearData)
        {
            //Note:
            //1st thing to think of how you are going to handle the keys
            //solution a) the key can be hardcoded
            //solution b) the key can be randomized/generated out of something the user's password
            //solution c) a password and salt which are hardcoded (which never change) out of which you generate "randomly" the key
            //in the assignment:
            //1 to encrypt/decrypt query string values
            //2 to encrypt/decrypt the file data as part of the hybrid encryption/decryption 

            //password will be the source of origin of our secret key
            //the salt will add more security against an attacker guessing the password using dictionary attacks

            //how do we generate a secret key
            //note: each algorithm has a different sized secret key


            //0 declare algorithm to use
            Rijndael myAlg = Rijndael.Create();
            //1.first we generate the secret key and iv
            var keys = GenerateKeys();

            MemoryStream msIn = new MemoryStream(clearData);
            msIn.Position = 0; //making sure that the pointer of the byte to read next is at the beginning so we encrypt everything

            MemoryStream msOut = new MemoryStream();

            //4.declaring a stream which handles data encryption
            CryptoStream cs = new CryptoStream(msOut, //the engine that operates encypting medium
                myAlg.CreateEncryptor(keys.SecretKey, keys.Iv), //this will write the data fed into the medium
                CryptoStreamMode.Write
                );

            //5. we start encrypting engine
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut
            cs.FlushFinalBlock();

            //7
            cs.Close();

            //8
            return msOut.ToArray();


        }
        public SymmetricKeys GenerateKeys()
        {
            //password + Salt >>>> algorithm >>> secret key + IV (which is the input needed by the encryption algorithm
            Rijndael myAlg = Rijndael.Create();

            Rfc2898DeriveBytes myGenerator = new Rfc2898DeriveBytes(password, salt);

            SymmetricKeys keys = new SymmetricKeys()
            {
                SecretKey = myGenerator.GetBytes(myAlg.KeySize / 8),
                Iv = myGenerator.GetBytes(myAlg.BlockSize / 8)
            };

            return keys;


        }

       // public string SymmetricEncrypt(string ClearData)
      // {

       // }


    }
}
