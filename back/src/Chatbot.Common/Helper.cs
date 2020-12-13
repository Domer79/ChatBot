using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Common
{
    public static class Helper
    {
        /// <summary>
        /// Генерация токена
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            var size = 100;
            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            var charSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var chars = charSet.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        
        /// <summary>
        /// Возвращает хэш SHA1 массива байтов
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] GetSHA1HashBytes(this byte[] bytes)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(bytes);
        }

        /// <summary>
        /// Возвращает хэш SHA1 введенной строки 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetSHA1HashBytes(this string value)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(value.GetBytes());
        }

        /// <summary>
        /// Преобразует строку в массив байтов
        /// </summary>
        /// <param name="value">Исходная строка</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string value)
        {
            return Encoding.Unicode.GetBytes(value);
        }

        public static async Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> source)
        {
            return (await source).ToArray();
        }

        public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> source)
        {
            return (await source).ToList();
        }
    }
}