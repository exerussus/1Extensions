using System.Runtime.CompilerServices;
using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class Vector2Extensions
    { 
        private const float TwoPI = 6.2831853f;
        
        /// <summary>
        /// Возвращает квадрат расстояния между двумя векторами <see cref="Vector2"/>.
        /// </summary>
        /// <param name="originPosition">Начальная позиция.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSqrDistance(this Vector2 originPosition, Vector2 targetPosition)
        {
            var dx = originPosition.x - targetPosition.x;
            var dy = originPosition.y - targetPosition.y;
            return dx * dx + dy * dy;
        }
        
        /// <summary>
        /// Пытается вычислить пересечение двух прямоугольников и получить точки Start, Center и End.
        /// </summary>
        /// <param name="aMin">Минимальная точка первого прямоугольника.</param>
        /// <param name="aMax">Максимальная точка первого прямоугольника.</param>
        /// <param name="bMin">Минимальная точка второго прямоугольника.</param>
        /// <param name="bMax">Максимальная точка второго прямоугольника.</param>
        public static (Vector2 start, Vector2 center, Vector2 end) GetIntersection(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax)
        {
            var startX = Mathf.Max(aMin.x, bMin.x);
            var endX = Mathf.Min(aMax.x, bMax.x);
            var centerX = (startX + endX) / 2;
            
            var startY = Mathf.Max(aMin.y, bMin.y);
            var endY = Mathf.Min(aMax.y, bMax.y);
            var centerY = (startY + endY) / 2;

            var maxPoint = new Vector2(Mathf.Max(startX, endX), Mathf.Max(startY, endY));
            var minPoint = new Vector2(Mathf.Min(startX, endX), Mathf.Min(startY, endY));
            
            return (minPoint, new Vector2(centerX, centerY), maxPoint);
        }
    
        /// <summary>
        /// Возвращает квадрат расстояния между вектором <see cref="Vector2"/> и <see cref="Vector3"/>, 
        /// при этом ось Z у Vector3 игнорируется.
        /// </summary>
        /// <param name="originPosition">Начальная позиция (Vector2).</param>
        /// <param name="targetPosition">Целевая позиция (Vector3). Используются только X и Y.</param>
        /// <returns>Квадрат расстояния между точками.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSqrDistance(this Vector2 originPosition, Vector3 targetPosition)
        {
            var dx = originPosition.x - targetPosition.x;
            var dy = originPosition.y - targetPosition.y;
            return dx * dx + dy * dy;
        }
        
        /// <summary>
        /// Возвращает случайную точку внутри круга заданного радиуса вокруг исходного вектора <see cref="Vector2"/>.
        /// </summary>
        /// <param name="origin">Центр круга.</param>
        /// <param name="radius">Радиус круга. Должен быть неотрицательным.</param>
        /// <returns>Случайная точка в пределах заданного радиуса.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetRandomPointInRadius(this Vector2 origin, float radius)
        {
            var angle = Random.value * TwoPI;
            var dist = Mathf.Sqrt(Random.value) * radius;

            FastSinCos(angle, out var sin, out var cos);

            return new Vector2(
                origin.x + cos * dist,
                origin.y + sin * dist
            );
        }        
        
        /// <summary>
        /// Быстрая аппроксимация синуса и косинуса.
        /// </summary>
        /// <param name="x">Угол в радианах.</param>
        /// <param name="sin">Рассчитанное значение синуса.</param>
        /// <param name="cos">Рассчитанное значение косинуса.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FastSinCos(float x, out float sin, out float cos)
        {
            var x2 = x * x;
            sin = x * (1f - 0.16605f * x2);
            cos = 1f - 0.5f * x2;
        }
        
        /// <summary>
        /// Возвращает случайную точку внутри квадрата с центром в заданной позиции <see cref="Vector2"/>.
        /// </summary>
        /// <param name="origin">Центр квадрата.</param>
        /// <param name="length">Половина длины стороны квадрата. Должен быть неотрицательным.</param>
        /// <returns>Случайная точка внутри квадрата.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetRandomPointInQuad(this Vector2 origin, float length)
        {
            var x = origin.x + (Random.value * 2f - 1f) * length;
            var y = origin.y + (Random.value * 2f - 1f) * length;
            return new Vector2(x, y);
        }
    }
}