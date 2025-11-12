using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class Vector2Extensions
    { 
        private const float TwoPI = 6.2831853f;
        
        public static string SerializeToString(this Vector2 vector)
        {
            return $"{vector.x},{vector.y}";
        }

        public static Vector2 DeserializeFromString(this string data)
        {
            var parts = data.Split(',');
            if (parts.Length != 2) throw new FormatException("Invalid Vector2 string format.");
            return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }

        public static byte[] SerializeToBytes(this Vector2 vector)
        {
            var bytes = new byte[sizeof(float) * 2];
            Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, bytes, 0, sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, bytes, sizeof(float), sizeof(float));
            return bytes;
        }

        public static Vector2 DeserializeFromBytes(this byte[] bytes)
        {
            if (bytes.Length != sizeof(float) * 2) throw new ArgumentException("Invalid byte array size for Vector2.");
            return new Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)));
        }
        
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
            return new Vector2(origin.x + FastCos(angle) * dist, origin.y + FastSin(angle) * dist);
        }        
        
        /// <summary>
        /// Быстрая аппроксимация синуса.
        /// </summary>
        /// <param name="x">Угол в радианах.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float FastSin(float x)
        {
            var x2 = x * x;
            return x * (1f - 0.16605f * x2);
        }    
        
        /// <summary> Быстрая аппроксимация косинуса. </summary>
        /// <param name="x">Угол в радианах.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float FastCos(float x)
        {
            var x2 = x * x;
            return 1f - 0.5f * x2;
        }
        
        /// <summary>
        /// Возвращает случайную точку внутри квадрата с центром в заданной позиции <see cref="Vector2"/>.
        /// </summary>
        /// <param name="origin">Центр квадрата.</param>
        /// <param name="length">Половина длины стороны квадрата. Должен быть неотрицательным.</param>
        /// <returns>Случайная точка внутри квадрата.</returns>
        public static Vector2 GetRandomPointInQuad(this Vector2 origin, float length)
        {
            var x = origin.x + (Random.value * 2f - 1f) * length;
            var y = origin.y + (Random.value * 2f - 1f) * length;
            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Возвращает случайную точку между двумя точками.
        /// </summary>
        /// <param name="firstPosition">Первая точка.</param>
        /// <param name="secondPosition">Вторая точка.</param>
        /// <returns>Случайная точка внутри квадрата.</returns>
        public static Vector2 GetRandomPointWith(this Vector2 firstPosition, Vector2 secondPosition)
        {
            var x = Random.Range(firstPosition.x, secondPosition.x);
            var y = Random.Range(firstPosition.y, secondPosition.y);
            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Возвращает вектор со смещением.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="offsetX">Смещение по оси X.</param>
        /// <param name="offsetY">Смещение по оси Y.</param>
        /// <returns>Вектор со смещением.</returns>
        public static Vector2 WithOffset(this Vector2 vector, float offsetX, float offsetY)
        {
            return new Vector2(vector.x + offsetX, vector.y + offsetY);
        }
        
        /// <summary>
        /// Возвращает вектор со смещением.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="offset">Смещение.</param>
        /// <returns>Вектор со смещением.</returns>
        public static Vector2 WithOffset(this Vector2 vector, Vector2 offset)
        {
            return new Vector2(vector.x + offset.x, vector.y + offset.y);
        }
        
        /// <summary>
        /// Возвращает вектор со смещением.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="offsetX">Смещение по оси X.</param>
        /// <param name="offsetY">Смещение по оси Y.</param>
        /// <returns>Вектор со смещением.</returns>
        public static Vector2Int WithOffset(this Vector2Int vector, int offsetX, int offsetY)
        {
            return new Vector2Int(vector.x + offsetX, vector.y + offsetY);
        }
        
        /// <summary>
        /// Возвращает вектор со смещением.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="offset">Смещение.</param>
        /// <returns>Вектор со смещением.</returns>
        public static Vector2Int WithOffset(this Vector2Int vector, Vector2Int offset)
        {
            return new Vector2Int(vector.x + offset.x, vector.y + offset.y);
        }
        
        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        public static Vector3 Abs(this Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        public static Vector2Int Abs(this Vector2Int vector)
        {
            return new Vector2Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }

        public static Vector3Int Abs(this Vector3Int vector)
        {
            return new Vector3Int(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }
        
        /// <summary> Возвращает вектор с обмененными значения X и Y. </summary>
        public static Vector2 Swap(this Vector2 vector)
        {
            return new Vector2(vector.y, vector.x);
        }

        /// <summary> Возвращает вектор с обмененными значения X и Y. </summary>
        public static Vector2Int Swap(this Vector2Int vector)
        {
            return new Vector2Int(vector.y, vector.x);
        }
    }
}