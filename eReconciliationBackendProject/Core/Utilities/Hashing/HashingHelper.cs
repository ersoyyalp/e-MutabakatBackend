using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Hashing
{
    public class HashingHelper
    { 
        public static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {                                                //Şifre hashleme
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {                                                   //Gönderdiğimiz şifreyi Hashlıyoruz kriptolu halını cekiyoruz
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {   //Gönderdiğimiz şifreyle mevcut hashleme arasındaki bağlantıyı contentlıyoruz
                //Yeni gönderiğimmiz şifrenin kriptolu haliyle harf harf karşılaştırıyoruz
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
