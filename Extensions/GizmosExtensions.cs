using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class GizmosExtensions
    {
        public static void DrawYLimits(Vector3 center, float yLimit, float radius)
        {
            DrawYChord(yLimit);
            DrawYChord(-yLimit);
            
            void DrawYChord(float targetY)
            {
                var originPosition = center;
                originPosition.y += targetY;
                var targetPosition = originPosition;
                var chordLength = GetChordLength(targetY);
                targetPosition.x += chordLength;
                Gizmos.DrawLine(originPosition, targetPosition);
                targetPosition.x -= chordLength * 2;
                Gizmos.DrawLine(originPosition, targetPosition);
            }

            float GetChordLength(float targetY)
            {
                return Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(targetY, 2));
            }
        }
        
        private static void DrawQuad(Vector3 center, float xQuadRadius, float yQuadRadius)
        {
            Vector3 topLeft = new Vector3(center.x - xQuadRadius, center.y + yQuadRadius, center.z);
            Vector3 topRight = new Vector3(center.x + xQuadRadius, center.y + yQuadRadius, center.z);
            Vector3 bottomRight = new Vector3(center.x + xQuadRadius, center.y - yQuadRadius, center.z);
            Vector3 bottomLeft = new Vector3(center.x - xQuadRadius, center.y - yQuadRadius, center.z);
            
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}