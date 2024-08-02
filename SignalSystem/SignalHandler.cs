
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem
{
    [CreateAssetMenu(menuName = "1Extensions/SignalHandler", fileName = "SignalHandler")]
    public class SignalHandler : ScriptableObject
    {
        public Signal Signal { get; private set; } = new Signal();

        private void Awake()
        {
            Signal = new();
        }
    }
}