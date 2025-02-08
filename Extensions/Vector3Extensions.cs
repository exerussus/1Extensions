using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        } 
        
        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x + (x ?? 0),vector.y + ( y ?? 0), vector.z + ( z ?? 0));
        } 

        /// <summary>
        /// Возвращает квадрат расстояния между двумя векторами <see cref="Vector3"/>.
        /// </summary>
        /// <param name="originPosition">Начальная позиция.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        public static float GetSqrDistance(this Vector3 originPosition, Vector3 targetPosition)
        {
            return (originPosition - targetPosition).sqrMagnitude;
        }

        /// <summary>
        /// Возвращает квадрат расстояния между двумя векторами <see cref="Vector3"/>.
        /// </summary>
        /// <param name="originPosition">Начальная позиция.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        public static float GetVector2SqrDistance(this Vector3 originPosition, Vector3 targetPosition)
        {
            var origin = new Vector2(originPosition.x, originPosition.y);
            var target = new Vector2(targetPosition.x, targetPosition.y);
            return (target - origin).sqrMagnitude;
        }

        /// <summary>
        /// Возвращает квадрат расстояния между двумя векторами <see cref="Vector3"/>.
        /// </summary>
        /// <param name="originPosition">Начальная позиция.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        public static float GetVector2SqrDistance(this Vector3 originPosition, Vector2 targetPosition)
        {
            var origin = new Vector2(originPosition.x, originPosition.y);
            return (targetPosition - origin).sqrMagnitude;
        }
    }
}