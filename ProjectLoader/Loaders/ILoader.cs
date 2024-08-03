using System;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Exerussus._1Extensions.Loader
{
    public interface ILoader
    {
        public T ResourceLoad<T>(string path) where T : Object;
        public Task<T> ResourceLoadAsync<T>(string path) where T : Object;
        public void ResourceLoadAsync<T>(string path, Action<T> onSuccess, Action<string> onFalse) where T : Object;
    }
}