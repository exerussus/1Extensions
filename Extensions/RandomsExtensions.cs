using System.Runtime.CompilerServices;
using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class RandomsExtensions
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
        /// <returns>True, если выпало значение меньше или равное value</returns>
        public static bool Roll100(this float value)
        {
            return Random.Range(1, 101) <= value;
        }

        /// <summary> Бросает детерминированный рандом между 1 и 100 включительно, используя seed. </summary>
        /// <param name="value">Текущий шанс.</param>
        /// <param name="seed">Зерно для генерации рандома.</param>
        /// <returns>True, если выпало значение меньше или равное value</returns>
        public static bool Roll100(this int value, int seed)
        {
            var random = new System.Random(seed);
            return random.Next(1, 101) <= value;
        }

        /// <summary> Бросает детерминированный рандом между 1 и 100 включительно, используя seed. </summary>
        /// <param name="value">Текущий шанс.</param>
        /// <param name="seed">Зерно для генерации рандома.</param>
        /// <returns>True, если выпало значение меньше или равное value</returns>
        public static bool Roll100(this float value, int seed)
        {
            var random = new System.Random(seed);
            return random.Next(1, 101) <= value;
        }

        /// <summary> Бросает рандом между 1 и 100 включительно несколько раз. </summary>
        /// <param name="value">Текущий шанс.</param>
        /// <param name="rollTimes">Сколько раз кидается ролл.</param>
        /// <param name="successfulRolls">Сколько успешно брошенных роллов.</param>
        /// <returns>True, если хотя бы один ролл был успешен.</returns>
        public static bool Roll100Many(this int value, int rollTimes, out int successfulRolls)
        {
            successfulRolls = 0;
            for (int i = 0; i < rollTimes; i++) if (Random.Range(1, 101) <= value) successfulRolls++;
            return successfulRolls > 1;
        }

        /// <summary>
        /// Возвращает новый <see cref="Vector3"/>, в котором координаты X и Y случайно изменены в пределах заданного отклонения.
        /// Координата Z остается без изменений.
        /// </summary>
        /// <param name="originValue">Исходное значение в виде <see cref="Vector3"/>.</param>
        /// <param name="offset">Максимальное отклонение для координат X и Y.</param>
        /// <returns>Новый <see cref="Vector3"/> с измененными координатами X и Y.</returns>
        public static Vector3 RandomOffsetXY(this Vector3 originValue, float offset)
        {
            return new Vector3
            {
                x = originValue.x.RandomOffset(offset),
                y = originValue.y.RandomOffset(offset),
                z = originValue.z
            };
        }

        /// <summary>
        /// Возвращает новый <see cref="Vector3"/>, в котором координата X случайно изменена в пределах заданного отклонения.
        /// Остальные координаты остаются без изменений.
        /// </summary>
        /// <param name="originValue">Исходное значение в виде <see cref="Vector3"/>.</param>
        /// <param name="offset">Максимальное отклонение для координаты X.</param>
        /// <returns>Новый <see cref="Vector3"/> с измененной координатой X.</returns>
        public static Vector3 RandomOffsetX(this Vector3 originValue, float offset)
        {
            return new Vector3
            {
                x = originValue.x.RandomOffset(offset),
                y = originValue.y,
                z = originValue.z
            };
        }

        /// <summary>
        /// Возвращает новый <see cref="Vector3"/>, в котором все координаты случайно изменены в пределах заданного отклонения.
        /// </summary>
        /// <param name="originValue">Исходное значение в виде <see cref="Vector3"/>.</param>
        /// <param name="offset">Максимальное отклонение для всех координат.</param>
        /// <returns>Новый <see cref="Vector3"/> с измененными координатами.</returns>
        public static Vector3 RandomOffset(this Vector3 originValue, float offset)
        {
            return new Vector3
            {
                x = originValue.x.RandomOffset(offset),
                y = originValue.y.RandomOffset(offset),
                z = originValue.z.RandomOffset(offset)
            };
        }

        /// <summary>
        /// Возвращает новый <see cref="Vector2"/>, в котором все координаты случайно изменены в пределах заданного отклонения.
        /// </summary>
        /// <param name="originValue">Исходное значение в виде <see cref="Vector2"/>.</param>
        /// <param name="offset">Максимальное отклонение для всех координат.</param>
        /// <returns>Новый <see cref="Vector2"/> с измененными координатами.</returns>
        public static Vector2 RandomOffset(this Vector2 originValue, float offset)
        {
            return new Vector2
            {
                x = originValue.x.RandomOffset(offset),
                y = originValue.y.RandomOffset(offset)
            };
        }

        /// <summary>
        /// Возвращает случайное значение в пределах диапазона, заданного исходным значением и отклонением.
        /// </summary>
        /// <param name="originValue">Исходное значение.</param>
        /// <param name="offset">Отклонение от исходного значения.</param>
        /// <returns>Случайное значение в диапазоне [originValue - offset, originValue + offset].</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float RandomOffset(this float originValue, float offset)
        {
            return Random.Range(originValue - offset, originValue + offset);
        }

        /// <summary>
        /// Возвращает случайное целое значение в пределах диапазона, заданного исходным значением и отклонением.
        /// </summary>
        /// <param name="originValue">Исходное значение.</param>
        /// <param name="offset">Отклонение от исходного значения.</param>
        /// <returns>Случайное целое значение в диапазоне [originValue - offset, originValue + offset + 1].</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int RandomOffset(this int originValue, int offset)
        {
            return Random.Range(originValue - offset, originValue + offset + 1);
        }
        
        /// <summary> Бросает рандом между 1 и 100 включительно. </summary>
        /// <param name="value">Текущий шанс.</param>
        /// <param name="result">Выпавшее значение между 1 и 100 включительно.</param>
        /// <returns>True, если выпало значение меньше или равное value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Roll100(this int value, out int result)
        {
            result = Random.Range(1, 101);
            return result <= value;
        }

        /// <summary> Бросает рандом между 1 и target включительно. </summary>
        /// <param name="current">Текущий шанс.</param>
        /// <param name="target">Максимальное число для рандома. Рандом кидается между 1 и target включительно.</param>
        /// <returns>True, если выпало значение меньше или равное current.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Roll(this int current, int target)
        {
            return Random.Range(1, target + 1) <= current;
        }

        /// <summary> Бросает рандом между 1 и target включительно. </summary>
        /// <param name="current">Текущий шанс.</param>
        /// <param name="target">Максимальное число для рандома. Рандом кидается между 1 и target включительно.</param>
        /// <param name="result">Выпавшее значение между 1 и target включительно.</param>
        /// <returns>True, если выпало значение меньше или равное current.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Roll(this int current, int target, out int result)
        {
            result = Random.Range(1, target + 1);
            return result <= current;
        }
    }
}