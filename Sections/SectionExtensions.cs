using Exerussus._1Extensions.GizmosExtensions;
using UnityEngine;

namespace Exerussus._1Extensions.Sections
{
    public static class SectionExtensions
    {
        public static void DrawGizmos(this Section section, Color lineColor, Transform parent = null)
        {
            var offset = parent == null ? Vector2.zero : (Vector2)parent.position;
            GizmosUtils.DrawQuad(offset + section.LeftBottom, offset + section.RightTop, lineColor);
        }
        
        public static void DrawGizmos(this Section section, Color lineColor, Color solidColor, Transform parent = null)
        {
            var offset = parent == null ? Vector2.zero : (Vector2)parent.position;
            GizmosUtils.DrawSolidQuad(offset + section.LeftBottom, offset + section.RightTop, lineColor, solidColor);
        }

        public static bool ContainPosition(this Section section, Vector2 position)
        {
            return section.LeftBottom.x <= position.x && position.x <= section.RightTop.x &&
                   section.LeftBottom.y <= position.y && position.y <= section.RightTop.y;
        }

        public static Vector2 GetRandomPosition(this Section section)
        {
            return new Vector2(Random.Range(section.LeftBottom.x, section.RightTop.x), Random.Range(section.LeftBottom.y, section.RightTop.y));
        }
    }
}