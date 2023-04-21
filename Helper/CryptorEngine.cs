using System.Security.Cryptography;
using System.Text;

namespace API_Tutorial.Helper
{
    public class CryptorEngine
    {

        static string key = "11h@tD21y";

    public static string Encrypt(string toEncrypt, string key, bool useHashing)
    {
        byte[] keyArray;
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

        if (useHashing)
        {
            using (var hashmd5 = MD5.Create())
            {
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
        else
        {
            keyArray = Encoding.UTF8.GetBytes(key);
        }

        using (var tdes = TripleDES.Create())
        {
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using (var cTransform = tdes.CreateEncryptor())
            {
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }
    }
    public static string Decrypt(string toDecrypt, string key, bool useHashing)
    {
        byte[] keyArray;
        byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);

        if (useHashing)
        {
            using (var hashmd5 = MD5.Create())
            {
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
        else
        {
            keyArray = Encoding.UTF8.GetBytes(key);
        }

        using (var tdes = TripleDES.Create())
        {
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using (var cTransform = tdes.CreateDecryptor())
            {
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }
    }

    }   

}