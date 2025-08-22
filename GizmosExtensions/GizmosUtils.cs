
using System.Diagnostics;
using UnityEngine;

namespace Exerussus._1Extensions.GizmosExtensions
{
    public static class GizmosUtils
    {
        private static readonly Color[] Colors = new[]
        {
            new Color(1, 0, 0, 1),
            new Color(1, 1, 0, 1),
            new Color(0, 1, 0, 1),
            new Color(0, 1, 1, 1),
            new Color(0, 0, 1, 1),
            new Color(1, 0, 1, 1),
            new Color(0.5f, 1, 0.5f, 1),
            new Color(1, 0.5f, 0.5f, 1),
            new Color(0.5f, 0.5f, 1, 1),
            new Color(1, 0.5f, 0, 1),
            new Color(0.5f, 1, 0, 1),
            new Color(1, 0.5f, 1, 1),
        };
        private static readonly Vector3 OneMinus = new Vector3(1, -1, 1);
        
#if UNITY_EDITOR
        private static readonly GUIStyle TextStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            fontSize = 30
        };
#endif

        [Conditional("UNITY_EDITOR")]
        public static void DrawText(string text, Vector3 position, Color color)
        {
#if UNITY_EDITOR
            TextStyle.normal.textColor = color;
            UnityEditor.Handles.Label(position, text, TextStyle); 
#endif
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawText(string text, Vector3 position, Color color, FontStyle fontStyle, int fontSize)
        {
#if UNITY_EDITOR
            TextStyle.normal.textColor = color;
            TextStyle.fontStyle = fontStyle;
            TextStyle.fontSize = fontSize;
            UnityEditor.Handles.Label(position, text, TextStyle); 
#endif
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(start, end);
            Gizmos.color = prevColor;
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawQuad(Vector2 minPosition, Vector2 maxPosition, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            
            var topLeft = new Vector3(minPosition.x, maxPosition.y);
            var bottomRight = new Vector3(maxPosition.x, minPosition.y);

            Gizmos.DrawLine(minPosition, bottomRight);
            Gizmos.DrawLine(minPosition, topLeft);
            Gizmos.DrawLine(topLeft, maxPosition);
            Gizmos.DrawLine(bottomRight, maxPosition);
            Gizmos.DrawLine(bottomRight, minPosition);
            Gizmos.color = prevColor;
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawSolidQuad(Vector2 minPosition, Vector2 maxPosition, Color lineColor, Color solidColor)
        {
            var prevColor = Gizmos.color;
            
            var topLeft = new Vector3(minPosition.x, maxPosition.y);
            var bottomRight = new Vector3(maxPosition.x, minPosition.y);

            Gizmos.color = solidColor;
            var center = (minPosition + maxPosition) / 2;
            var size = new Vector3(maxPosition.x - minPosition.x, maxPosition.y - minPosition.y, 0f);
            Gizmos.DrawCube(center, size);
            
            Gizmos.color = lineColor;
            Gizmos.DrawLine(minPosition, bottomRight);
            Gizmos.DrawLine(minPosition, topLeft);
            Gizmos.DrawLine(topLeft, maxPosition);
            Gizmos.DrawLine(bottomRight, maxPosition);
            Gizmos.DrawLine(bottomRight, minPosition);
            Gizmos.color = prevColor;
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawCircle(Vector2 center, float radius, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(center, radius);
            Gizmos.color = prevColor;
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawArrow(Vector3 startPoint, Vector3 endPoint, Color color, float arrowHeadLength = 0.15f)
        {
            
            var prevColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(startPoint, endPoint);
            
            var direction = (endPoint - startPoint).normalized;

            var right = (Vector3)Rotate(direction, 170f);
            var left = (Vector3)Rotate(direction, -170f);

            Gizmos.DrawLine(endPoint, endPoint + right * arrowHeadLength);
            Gizmos.DrawLine(endPoint, endPoint + left * arrowHeadLength);

            Gizmos.color = prevColor;
        }
        
        private static Vector2 Rotate(Vector2 v, float degrees)
        {
            var rad = degrees * Mathf.Deg2Rad;
            var cos = Mathf.Cos(rad);
            var sin = Mathf.Sin(rad);

            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }
        
        public static Color GetColor(int index)
        {
            return Colors[GetIndexInSize(index, Colors.Length)];
        }

        private static int GetIndexInSize(int index, int size)
        {
            if (index >= size) return index % size;
            return index;
        }
    }
}