using UnityEngine;

namespace Exerussus._1Extensions.GizmosExtensions
{
    public static class GizmosUtils
    {
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(start, end);
            Gizmos.color = prevColor;
        }

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
    }
}