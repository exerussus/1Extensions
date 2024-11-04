using System;
using System.Diagnostics;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true), Conditional("UNITY_EDITOR")]
    public class AnchorAttribute : PropertyAttribute
    {
        
    }

}