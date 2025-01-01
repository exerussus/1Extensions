using Exerussus._1Extensions.GizmosExtensions;
using UnityEngine;

namespace Exerussus._1Extensions.Sections
{
    public static class SectionExtensions
    {
        public static void DrawGizmos(this Section section, Color lineColor, Transform parent = null)
        {
            var offset = parent == null ? Vector2.zero : (Vector2)parent.position;
            GizmosUtils.DrawQuad(offset + section.minPosition, offset + section.maxPosition, lineColor);
        }
        
        public static void DrawGizmos(this Section section, Color lineColor, Color solidColor, Transform parent = null)
        {
            var offset = parent == null ? Vector2.zero : (Vector2)parent.position;
            GizmosUtils.DrawSolidQuad(offset + section.minPosition, offset + section.maxPosition, lineColor, solidColor);
        }
    }
}