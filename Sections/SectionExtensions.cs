using Exerussus._1Extensions.GizmosExtensions;
using UnityEngine;

namespace Exerussus._1Extensions.Sections
{
    public static class SectionExtensions
    {
        public static void DrawGizmos(this Section section, Color lineColor)
        {
            GizmosUtils.DrawQuad(section.minPosition, section.maxPosition, lineColor);
        }
        
        public static void DrawGizmos(this Section section, Color lineColor, Color solidColor)
        {
            GizmosUtils.DrawSolidQuad(section.minPosition, section.maxPosition, lineColor, solidColor);
        }
    }
}