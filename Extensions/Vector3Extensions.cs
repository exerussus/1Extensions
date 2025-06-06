﻿using System;
using System.Globalization;
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
        
        public static string SerializeToString(this Vector3 vector)
        {
            return $"{vector.x.ToString(CultureInfo.InvariantCulture)},{vector.y.ToString(CultureInfo.InvariantCulture)},{vector.z.ToString(CultureInfo.InvariantCulture)}";
        }

        public static Vector3 DeserializeFromString(this string data)
        {
            var parts = data.Split(',');
            if (parts.Length != 3) throw new FormatException("Invalid Vector3 string format.");
            return new Vector3(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture), float.Parse(parts[2], CultureInfo.InvariantCulture));
        }
        
        public static bool TryDeserializeVector3FromString(this string data, out Vector3 result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(data)) return false;

            var parts = data.Split(',');
            if (parts.Length != 3) return false;

            bool parsedX = float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x);
            bool parsedY = float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y);
            bool parsedZ = float.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float z);

            if (parsedX && parsedY && parsedZ)
            {
                result = new Vector3(x, y, z);
                return true;
            }

            return false;
        }
        
        public static bool TryDeserializeFromString(ref Vector3 result, string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return false;

            var parts = data.Split(',');
            if (parts.Length != 3) return false;

            bool parsedX = float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x);
            bool parsedY = float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y);
            bool parsedZ = float.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float z);

            if (parsedX && parsedY && parsedZ)
            {
                result.x = x;
                result.y = y;
                result.z = z;
                return true;
            }

            return false;
        }
        
        public static bool TryDeserializeFromString(string data, out Vector3 result)
        {
            result = new Vector3();
            
            if (string.IsNullOrWhiteSpace(data)) return false;

            var parts = data.Split(',');
            if (parts.Length != 3) return false;

            bool parsedX = float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x);
            bool parsedY = float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y);
            bool parsedZ = float.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float z);

            if (parsedX && parsedY && parsedZ)
            {
                result.x = x;
                result.y = y;
                result.z = z;
                return true;
            }

            return false;
        }

        public static byte[] SerializeToBytes(this Vector3 vector)
        {
            var bytes = new byte[sizeof(float) * 3];
            Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, bytes, 0, sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, bytes, sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(vector.z), 0, bytes, sizeof(float) * 2, sizeof(float));
            return bytes;
        }

        public static Vector3 DeserializeFromBytes(this byte[] bytes)
        {
            if (bytes.Length != sizeof(float) * 3) throw new ArgumentException("Invalid byte array size for Vector3.");
            return new Vector3(
                BitConverter.ToSingle(bytes, 0),
                BitConverter.ToSingle(bytes, sizeof(float)),
                BitConverter.ToSingle(bytes, sizeof(float) * 2)
            );
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