using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class TimeFormatter
    {
        /// <summary>
        /// Преобразует время в секундах в строку формата:
        /// "дней: {days}, часов: {hours}, минут: {minutes}, секунд: {seconds}" (если прошло хотя бы 1 день),
        /// "часов: {hours}, минут: {minutes}, секунд: {seconds}" (если прошло менее 1 дня, но есть часы),
        /// "минут: {minutes}, секунд: {seconds}" (если прошло менее часа, но есть минуты),
        /// "секунд: {seconds}" (если прошло меньше минуты).
        /// </summary>
        /// <param name="time">Время в секундах (например, Time.time).</param>
        /// <returns>Отформатированная строка.</returns>
        public static string FormatTime(float time)
        {
            var totalSeconds = Mathf.FloorToInt(time);
            var days = totalSeconds / 86400;                // 86400 секунд = 1 день
            var hours = totalSeconds % 86400 / 3600;        // 3600 секунд = 1 час
            var minutes = totalSeconds % 3600 / 60;
            var seconds = totalSeconds % 60;

            if (days > 0)
            {
                return $"дней: {days}, часов: {hours}, минут: {minutes}, секунд: {seconds}";
            }
            else if (hours > 0)
            {
                return $"часов: {hours}, минут: {minutes}, секунд: {seconds}";
            }
            else if (minutes > 0)
            {
                return $"минут: {minutes}, секунд: {seconds}";
            }
            else
            {
                return $"секунд: {seconds}";
            }
        }
    }
}