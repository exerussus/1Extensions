
using System;
using System.Reflection;

namespace Exerussus._1Extensions.ReflectionHelper
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Устанавливает значение свойства даже если у него нет публичного сеттера.
        /// </summary>
        /// <param name="target">Объект, в котором находится свойство.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение для установки.</param>
        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var type = target.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property == null)
                throw new ArgumentException($"Свойство '{propertyName}' не найдено в типе {type.FullName}.");

            // Пробуем найти auto-property backing field
            var backingField = type.GetField($"<{property.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);

            if (backingField != null)
            {
                backingField.SetValue(target, value);
                return;
            }

            // Если backing field не найден — используем сеттер (возможно, приватный)
            var setMethod = property.GetSetMethod(true);
            if (setMethod != null)
            {
                setMethod.Invoke(target, new[] { value });
                return;
            }

            throw new InvalidOperationException(
                $"Не удалось установить значение свойства '{property.Name}' — отсутствует сеттер и бэкенд-поле.");
        }

        public static Action<object> CreateSetPropertyAction(object target, string propertyName)
        {            
            if (target == null) throw new ArgumentNullException(nameof(target));
            var type = target.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property == null) throw new ArgumentException($"Свойство '{propertyName}' не найдено в типе {type.FullName}.");
            var backingField = type.GetField($"<{property.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            if (backingField != null)
            {
                return value => backingField.SetValue(target, value);
            }
            var setMethod = property.GetSetMethod(true);
            if (setMethod != null)
            {
                return value => setMethod.Invoke(target, new [] { value });
            }
            throw new InvalidOperationException($"Не удалось создать делегат, '{property.Name}' — отсутствует сеттер и бэкенд-поле.");
        }
    }

}