using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    [CreateAssetMenu(menuName = "1Extensions/GameShareHandler", fileName = "GameShareHandler")]
    public class GameShareHandler : ScriptableObject
    {
        [SerializeField] private List<GameShare> gameShares;
        private Dictionary<int, GameShare> _gameShares = new();
        
        public GameShare GetGameShare(int id)
        {
            if (!_gameShares.TryGetValue(id, out var gameShare))
            {
                gameShare = new GameShare();
                gameShares.Add(gameShare);
                _gameShares.Add(id, gameShare);
            }

            return gameShare;
        }
        
        public void Clear(int id)
        {
            if (!_gameShares.TryGetValue(id, out var gameShare)) return;
            gameShares.Remove(gameShare);
            _gameShares.Remove(id);
        }
        
        [UnityEditor.InitializeOnLoad]
        private class StaticCleaner
        {
            static StaticCleaner()
            {
                UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    string[] assetGuids = UnityEditor.AssetDatabase.FindAssets("t:GameShareHandler");
                    
                    foreach (string guid in assetGuids)
                    {
                        var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                        var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameShareHandler>(path);
                
                        if (asset != null)
                        {
                            asset._gameShares.Clear();
                            asset.gameShares.Clear();
                        }
                    }
                }
            }
        }
    }
}