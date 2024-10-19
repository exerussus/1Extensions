
using Plugins.Exerussus._1Extensions.Abstractions;
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem
{
    [CreateAssetMenu(menuName = "1Extensions/SignalHandler", fileName = "SignalHandler")]
    public class SignalHandler : ScriptableObject, IInitializable
    {
        public Signal Signal { get; private set; } = new Signal();

        private void Awake() { Initialize(); }
        public void Initialize() { Signal = new(); }
    }
}