using System;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    [Serializable]
    public class NamedVector2
    {
        public string name;
        public Vector2 value;
    }
    
    [Serializable]
    public class NamedVector3
    {
        public string name;
        public Vector3 value;
    }
}