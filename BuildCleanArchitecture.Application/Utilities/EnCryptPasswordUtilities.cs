using System.Security.Cryptography;
using System.Text;

namespace BuildCleanArchitecture.Application.Utilities
{
    public class EnCryptPasswordUtilities
    {
        private const string KEY_DECRYPT_ENCRYPT = "123";

        public static string EnCrypt(string strEnCrypt)
        {
            if (!string.IsNullOrEmpty(strEnCrypt))
            {
                try
                {
                    using var MD5Hash = new MD5CryptoServiceProvider();

                    byte[] keyArr = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(KEY_DECRYPT_ENCRYPT)); ;
                    byte[] EnCryptArr = Encoding.UTF8.GetBytes(strEnCrypt);

                    using var tripDes = new TripleDESCryptoServiceProvider
                    {
                        Key = keyArr,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    };

                    var transform = tripDes.CreateEncryptor();
                    byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);

                    return Convert.ToBase64String(arrResult, 0, arrResult.Length);
                }
                catch
                {
                    return null!;
                }
            }

            return null!;
        }
    }
}
