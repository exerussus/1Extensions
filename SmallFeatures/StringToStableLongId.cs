using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class StringToStableLongId
    {
        private static Dictionary<long, string> _dictionary = new();
        
        /// <summary>
        /// Возвращает стабильный уникальный long-идентификатор для строки.
        /// Использует SHA256. Идентификатор не зависит от платформы или времени запуска.
        /// </summary>
        public static long GetStableId(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Берём первые 8 байт (little-endian по умолчанию)
            var result = BitConverter.ToInt64(hash, 0);

            return result;
        }
        
        public static long GetStableLongId(this string input)
        {
            var result = GetStableId(input);
            _dictionary[result] = input;
            return result;
        }
        
        public static bool TryGetString(this long id, out string result) => _dictionary.TryGetValue(id, out result);
    }
}