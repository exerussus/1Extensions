
using Exerussus._1Extensions.Abstractions;
using Exerussus._1Extensions.SmallFeatures;
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem
{
    [CreateAssetMenu(menuName = "Exerussus/1Extensions/SignalHandler", fileName = "SignalHandler")]
    public class SignalHandler : ScriptableObject, IInitializable
    {
        public Signal Signal { get => SignalQoL.Instance; private set => SignalQoL.Instance = value; }

        private void Awake()
        {
            
        }

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }
    }
}