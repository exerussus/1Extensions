using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class Vector2Extensions
    { 
        /// <summary>
        /// Возвращает квадрат расстояния между двумя векторами <see cref="Vector2"/>.
        /// </summary>
        /// <param name="originPosition">Начальная позиция.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        public static float GetSqrDistance(this Vector2 originPosition, Vector2 targetPosition)
        {
            return (originPosition - targetPosition).sqrMagnitude;
        }

        /// <summary>
        /// Возвращает квадрат расстояния между вектором <see cref="Vector2"/> и <see cref="Vector3"/>, 
        /// при этом ось Z у Vector3 игнорируется.
        /// </summary>
        /// <param name="originPosition">Начальная позиция (Vector2).</param>
        /// <param name="targetPosition">Целевая позиция (Vector3). Используются только X и Y.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        public static float GetSqrDistance(this Vector2 originPosition, Vector3 targetPosition)
        {
            var target2D = new Vector2(targetPosition.x, targetPosition.y);
            return (originPosition - target2D).sqrMagnitude;
        }
    }
}