using System;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class EnumExtensions
    {
        private static readonly Random _random = new();

        /// <summary> Возвращает случайное значение для перечисления типа TEnum. </summary>
        public static TEnum GetRandomValue<TEnum>() where TEnum : Enum
        {
            var values = (TEnum[])Enum.GetValues(typeof(TEnum));
            return values[_random.Next(values.Length)];
        }

        /// <summary> Устанавливает случайное значение для переданного enum по ссылке. </summary>
        public static void SetRandomValue<TEnum>(this ref TEnum enumValue) where TEnum : struct, Enum
        {
            enumValue = GetRandomValue<TEnum>();
        }
        
        /// <summary> Увеличивает значение enum циклически (через ref), возвращаясь к первому при достижении конца. </summary>
        public static void NextEnumValue<TEnum>(this ref TEnum current) where TEnum : struct, Enum
        {
            var values = (TEnum[])Enum.GetValues(typeof(TEnum));
            var index = Array.IndexOf(values, current);
            index = (index + 1) % values.Length;
            current = values[index];
        }
    }
}