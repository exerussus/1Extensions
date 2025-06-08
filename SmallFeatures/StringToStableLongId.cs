using System;
using System.Security.Cryptography;
using System.Text;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class StringToStableLongId
    {
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
    }
}