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
        /// <param name="start">Верхняя левая точка пересечения.</param>
        /// <param name="center">Центральная точка между Start и End.</param>
        /// <param name="end">Нижняя левая точка пересечения.</param>
        /// <param name="force">Если true - точки высчитываются, даже без явного пересечения с выстраиванием проекционных линий.</param>
        /// <returns><c>true</c>, если прямоугольники пересекаются; иначе <c>false</c>.</returns>
        public static bool TryGetIntersection(
            Vector2 aMin, Vector2 aMax,
            Vector2 bMin, Vector2 bMax,
            out Vector2 start, out Vector2 center, out Vector2 end,
            bool force = false)
        {
            var xMin = Mathf.Max(aMin.x, bMin.x);
            var yMin = Mathf.Max(aMin.y, bMin.y);
            var xMax = Mathf.Min(aMax.x, bMax.x);
            var yMax = Mathf.Min(aMax.y, bMax.y);

            if (xMin < xMax && yMin < yMax)
            {
                start = new Vector2(xMin, yMax);
                end = new Vector2(xMin, yMin);
                center = (start + end) * 0.5f;
                return true;
            }

            if (force)
            {
                Vector2 aCenter = (aMin + aMax) * 0.5f;
                Vector2 bCenter = (bMin + bMax) * 0.5f;

                var dx = Mathf.Abs(aCenter.x - bCenter.x);
                var dy = Mathf.Abs(aCenter.y - bCenter.y);

                if (dx > dy)
                {
                    float x1 = aCenter.x < bCenter.x ? aMax.x : aMin.x;
                    float x2 = aCenter.x < bCenter.x ? bMin.x : bMax.x;
                    float y = (aCenter.y + bCenter.y) * 0.5f;
                    start = new Vector2(x1, y);
                    end = new Vector2(x2, y);
                }
                else
                {
                    float y1 = aCenter.y < bCenter.y ? aMax.y : aMin.y;
                    float y2 = aCenter.y < bCenter.y ? bMin.y : bMax.y;
                    float x = (aCenter.x + bCenter.x) * 0.5f;
                    start = new Vector2(x, y1);
                    end = new Vector2(x, y2);
                }

                center = (start + end) * 0.5f;
                return false;
            }

            start = center = end = Vector2.zero;
            return false;
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