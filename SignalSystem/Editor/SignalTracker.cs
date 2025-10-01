#if UNITY_EDITOR

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem.Editor
{
    public class SignalTracker : OdinEditorWindow
    {
        private long _lastHash = 0;
        [ShowInInspector] internal Dictionary<long, SignalStatistic> _signals => Editor.SignalManager._signals;

        [MenuItem("Tools/Signal Tacker")] 
        public static void OpenWindow()
        {
            GetWindow<SignalTracker>().Show();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
            if (SignalManager.StateHash == _lastHash) return;
            
            _lastHash = SignalManager.StateHash;
            Draw();
        }

        private void Draw()
        {
            
        }
    }
}

#endif