using System.Security.Cryptography;
using System.Text;

namespace Project.Common.Utility
{
    public static class EncryptionUtil
    {
        public static readonly int EncKeySize = 20;
        //public static readonly string EncFilePath = new DirectoryInfo(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent.FullName + "\\Uploads\\data\\Google.Apis.Util.dll";
        public static readonly string EncFilePath = string.Format("{0}{1}Uploads{1}data{1}License.lic", Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar);

        public static Dictionary<string, int> DecryptFile(string fileSourceEncrypted, char itemSeparator, char keyValueSeparator, int keySize)
        {
            byte[] allBytes = File.ReadAllBytes(fileSourceEncrypted);
            byte[] bytesToBeDecrypted = allBytes.Skip(keySize).ToArray();
            var keyBytes = allBytes.Take(keySize).ToArray();
            byte[] passwordBytes = SHA256.Create().ComputeHash(keyBytes);
            byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);
            string str = System.Text.Encoding.Default.GetString(bytesDecrypted);
            Dictionary<string, int> keyValuePairs = str.Split(itemSeparator)
                      .Select(value => value.Split(keyValueSeparator))
                      .ToDictionary(pair => pair[0], pair => Convert.ToInt32(pair[1]));
            return keyValuePairs;
        }

        public static Dictionary<string, int> DecryptFromStream(byte[] sourceBytes, char itemSeparator, char keyValueSeparator, int keySize)
        {
            byte[] allBytes = sourceBytes;
            byte[] bytesToBeDecrypted = allBytes.Skip(keySize).ToArray();
            var keyBytes = allBytes.Take(keySize).ToArray();
            byte[] passwordBytes = SHA256.Create().ComputeHash(keyBytes);
            byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);
            string str = System.Text.Encoding.Default.GetString(bytesDecrypted);
            Dictionary<string, int> keyValuePairs = str.Split(itemSeparator)
                      .Select(value => value.Split(keyValueSeparator))
                      .ToDictionary(pair => pair[0], pair => Convert.ToInt32(pair[1]));
            return keyValuePairs;
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        public static string Encrypt(string plainText, string password)
        {
            byte[] encryptedBytes = null;

            // Get the bytes of the string
            byte[] originalBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256  
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Getting the salt size  
            int saltSize = GetSaltSize(passwordBytes);
            // Generating salt bytes  
            byte[] saltBytes = GetRandomBytes(saltSize);

            // Appending salt bytes to original bytes  
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static string Decrypt(string encryptedText, string password)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = String.Empty;
            }

            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256  
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = Decrypt(bytesToBeDecrypted, passwordBytes);

            if (decryptedBytes != null)
            {
                // Getting the size of salt  
                int saltSize = GetSaltSize(passwordBytes);

                // Removing salt bytes, retrieving original bytes  
                byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
                for (int i = saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - saltSize] = decryptedBytes[i];
                }
                return Encoding.UTF8.GetString(originalBytes);
            }

            return null;
        }

        /// <summary>
        /// MD5 Hasher Encryption
        /// </summary>
        /// <param name="toencrypt"></param>
        /// <param name="key"></param>
        /// <param name="usehashing"></param>
        /// <returns></returns>
        public static string Encrypt(this string toEncrypt, string key = "EzyBanking", bool useHashing = true)
        {
            byte[] keyArray;
            if (useHashing)
            {
                using (var hashMd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }
            using (var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            using (var transform = tdes.CreateEncryptor())
            {
                try
                {
                    var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
                    var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// MD5 Hasher Decryption
        /// </summary>
        /// <param name="todecrypt"></param>
        /// <param name="key"></param>
        /// <param name="usehashing"></param>
        /// <returns></returns>
        public static string Decrypt(this string todEncrypt, string key = "EzyBanking", bool useHashing = true)
        {
            byte[] toEncryptArray;

            try
            {
                toEncryptArray = Convert.FromBase64String(todEncrypt.Replace(" ", "+"));
            }
            catch (Exception)
            {
                return String.Empty;
            }

            byte[] keyArray;

            if (useHashing)
            {
                using (var hashMd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }
            using (var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            using (var transform = tdes.CreateDecryptor())
            {
                try
                {
                    var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Encoding.UTF8.GetString(resultArray);
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }
            }
        }

        public static string GetHashedPassword(string password)
        {
            var hashedPassword = string.Empty;
            if (!string.IsNullOrEmpty(password))
            {
                var sha1 = SHA1.Create();
                var hashData = sha1.ComputeHash(Encoding.Default.GetBytes(password));
                var passwordBuilder = new StringBuilder();
                foreach (var bits in hashData)
                {
                    passwordBuilder.Append(bits.ToString());
                }
                hashedPassword = passwordBuilder.ToString();
            }
            return hashedPassword;
        }

        public static string GeneratePassword(int length) //length of salt    
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];

            for (var i = 0; i <= length - 1; i++)
            {
                chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public static string EncodePassword(string pass, string salt) //encrypt password    
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            //return Convert.ToBase64String(inArray);    
            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }

        public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            MD5 md5 = new MD5CryptoServiceProvider();
            originalBytes = Encoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }

        public static string Md5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var bits in bytes)
            {
                hash.Append(bits.ToString("x2"));
            }

            return hash.ToString();
        }

        public static string Base64Encode(string sData) // Encode    
        {
            try
            {
                byte[] encDataByte = new byte[sData.Length];
                encDataByte = Encoding.UTF8.GetBytes(sData);
                return Convert.ToBase64String(encDataByte);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public static string Base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);

                return new String(decodedChar);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }
        public static string Base64TextDecode(string text)
        {
            byte[] data = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(data);
        }

        public static bool TryBase64Decode(string encodedText, out string plainText)
        {
            plainText = string.Empty;
            try
            {
                plainText = Base64TextDecode(encodedText);
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return false;
        }

        // Function to hash the passwords
        public static string HashPassword(string message)
        {
            // Create a new instance of the SHA-256, 
            // you should use SHA256Managed instead of SHA256 object.
            using (var algo = new SHA256Managed())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(message);
                var hashedBytes = algo.ComputeHash(bytes);

                // The hash has been computed, to convert those bytes to string
                // I will use the following code, you may use your own
                // code to convert the byte array to string
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                foreach (byte bite in hashedBytes)
                {
                    builder.Append(bite.ToString("x2"));
                }

                // Return it to the caller, to write it.
                return builder.ToString();
            }
        }

        private static int GetSaltSize(byte[] passwordBytes)
        {
            var key = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }
        public static byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }
    }
}
