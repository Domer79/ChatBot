using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Abstractions.Contracts;

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

        public static EnumInfo[] GetEnumInfos(Type enumType)
        {
            var fieldInfos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            return fieldInfos.Select(_ =>
                {
                    var descriptionAttribute =
                        (DescriptionAttribute) _.GetCustomAttribute(typeof(DescriptionAttribute));
                    return new EnumInfo()
                    {
                        Name = _.Name,
                        Enum = (Enum) _.GetValue(null),
                        Description = descriptionAttribute?.Description
                    };
                })
                .ToArray();
        }

        public static EnumInfo<T>[] GetEnumInfos<T>() where T : Enum
        {
            return GetEnumInfos(typeof(T))
                .Select(_ => new EnumInfo<T>(_) {Value = (T) _.Enum})
                .ToArray();
        }

        public static bool IsEnum(Type enumType, Enum value)
        {
            var enumInfos = GetEnumInfos(enumType);
            return enumInfos.Any(enumInfo => enumInfo.Enum.Equals(value));
        }

        public static bool IsEnum<T>(T value) where T : Enum
        {
            var enumInfos = GetEnumInfos<T>();
            return enumInfos.Any(enumInfo => enumInfo.Value.Equals(value));
        }
        
        public static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }

        public static IEnumerable<TEnum> GetFlags<TEnum>(TEnum input) where TEnum: Enum
        {
            return GetFlags((Enum)input).Cast<TEnum>();
        }

        public static string GetLogin(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(_ => _.Type == CustomClaimTypes.Login);
            return claim?.Value;
        }

        public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(_ => _.Type == CustomClaimTypes.UserId);
            return claim == null ? null : Guid.Parse(claim.Value);
        }

        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }
    }
}