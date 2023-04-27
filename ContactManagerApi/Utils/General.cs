using System.Text;
using System.Security.Cryptography;

namespace ContactManagerApi.Utils
{
    public class General
    {
        public static string getSha1(string pCadena)
        {
            try
            {
                string cEncrypt;
                pCadena = pCadena.Trim();
                var sha1 = SHA1.Create();

                var encoding = new ASCIIEncoding();
                byte[] stream = null;
                var sb = new StringBuilder();
                stream = sha1.ComputeHash(encoding.GetBytes(pCadena));

                for (int i = 0; i < stream.Length; i++)
                {
                    sb.AppendFormat("{0:X2}", stream[i]);
                }

                cEncrypt = sb.ToString();
                return cEncrypt;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }
    }
}
