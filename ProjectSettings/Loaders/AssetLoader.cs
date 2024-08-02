using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Exerussus._1Extensions
{
    public class AssetLoader
    {
        public AssetLoader(ILoader assetLoader)
        {
            Loader = assetLoader;
        }
            
        private readonly ILoader Loader;
            
        public T LoadResource<T>(string path) where T : Object
        {
            return Loader.ResourceLoad<T>(path);
        }
            
        public void LoadResourceAsync<T>(string path, Action<T> onSuccess, Action<string> onFalse) where T : Object
        {
            Loader.ResourceLoadAsync(path, onSuccess, onFalse);
        } 
            
        public async Task<T> LoadResourceAsync<T>(string path) where T : Object
        {
            var task = await Loader.ResourceLoadAsync<T>(path);
            return task;
        } 
            
        /// <summary>
        ///  Работает ТОЛЬКО в Unity Editor. Загружает дату по пути Assets/Source/Data/ по умолчанию.
        /// </summary>
        public T GetAssetFromResourceByName<T>() where T : Object
        {
            return Resources.Load<T>(typeof(T).Name);
        }
    }
}