using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
    }
}