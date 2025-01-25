using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class GameExtensions
    {
        /// <summary> Бросает рандом между 1 и 100 включительно. </summary>
        /// <param name="value">Текущий шанс.</param>
        /// <returns>True, если выпало значение меньше или равное value</returns>
        public static bool Roll100(this int value)
        {
            return Random.Range(1, 101) <= value;
        }

        /// <summary> Бросает рандом между 1 и 100 включительно. </summary>
        /// <param name="value">Текущий шанс.</param>
        /// <param name="result">Выпавшее значение между 1 и 100 включительно.</param>
        /// <returns>True, если выпало значение меньше или равное value</returns>
        public static bool Roll100(this int value, out int result)
        {
            result = Random.Range(1, 101);
            return result <= value;
        }

        /// <summary> Бросает рандом между 1 и target включительно. </summary>
        /// <param name="current">Текущий шанс.</param>
        /// <param name="target">Максимальное число для рандома. Рандом кидается между 1 и target включительно.</param>
        /// <returns>True, если выпало значение меньше или равное current.</returns>
        public static bool Roll(this int current, int target)
        {
            return Random.Range(1, target + 1) <= current;
        }

        /// <summary> Бросает рандом между 1 и target включительно. </summary>
        /// <param name="current">Текущий шанс.</param>
        /// <param name="target">Максимальное число для рандома. Рандом кидается между 1 и target включительно.</param>
        /// <param name="result">Выпавшее значение между 1 и target включительно.</param>
        /// <returns>True, если выпало значение меньше или равное current.</returns>
        public static bool Roll(this int current, int target, out int result)
        {
            result = Random.Range(1, target + 1);
            return result <= current;
        }
    }
}