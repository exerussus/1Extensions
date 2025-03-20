using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.Pools
{
    public class SortedObjectPool<T> where T : Component
    {
        private GameObject _prefab;
        private const int DefaultObjectCount = 5;
        private Queue<T> FreeObjects { get; } = new();
        private Transform _parent;
        private string _elementName;
        private int _count;

        public Transform Parent => _parent;
        
        public void InitPrefab(GameObject prefab, bool dontDestroyOnLoad, int count = DefaultObjectCount)
        {
            _parent = new GameObject { name = $"{prefab.name} pool" }.transform;
            
            if (dontDestroyOnLoad) Object.DontDestroyOnLoad(_parent);
            
            _prefab = Object.Instantiate(prefab, _parent);
            _prefab.SetActive(false);
            _prefab.name = $"{prefab.name} prefab";
            _elementName = $"{prefab.name} element";
            for (int i = 0; i < count; i++)
            {
                var element = CreateNewObject();
                element.gameObject.SetActive(false);
                FreeObjects.Enqueue(element);
            }
        }

        public void InitPrefab(GameObject prefab, bool dontDestroyOnLoad, Transform parent, int count = DefaultObjectCount)
        {
            _parent = new GameObject { name = $"{prefab.name} pool" , transform = { parent = parent}}.transform;
            
            if (dontDestroyOnLoad) Object.DontDestroyOnLoad(_parent);
            
            _prefab = Object.Instantiate(prefab, _parent);
            _prefab.SetActive(false);
            _prefab.name = $"{prefab.name} prefab";
            _elementName = $"{prefab.name} element";
            for (int i = 0; i < count; i++)
            {
                var element = CreateNewObject();
                element.gameObject.SetActive(false);
                FreeObjects.Enqueue(element);
            }
        }

        public T GetObject(Vector3 position)
        {
            var pooledObject = GetOrCreate();
            pooledObject.transform.SetPositionAndRotation(position, Quaternion.identity);
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public T GetObject(Vector3 position, Quaternion rotation)
        {
            var pooledObject = GetOrCreate();
            pooledObject.transform.SetPositionAndRotation(position, rotation);
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public T GetObject(Vector3 position, Vector2 scale, Quaternion rotation)
        {
            var pooledObject = GetOrCreate();
            pooledObject.transform.SetPositionAndRotation(position, rotation);
            pooledObject.gameObject.transform.localScale = scale;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public T GetObject(Vector3 position, Vector2 scale)
        {
            var pooledObject = GetOrCreate();
            pooledObject.transform.SetPositionAndRotation(position, Quaternion.identity);
            pooledObject.gameObject.transform.localScale = scale;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public T GetObject(Vector3 position, Transform parent, Quaternion rotation)
        {
            var pooledObject = GetOrCreate();
            pooledObject.transform.SetParent(parent);
            pooledObject.transform.SetPositionAndRotation(position, rotation);
            pooledObject.gameObject.transform.localScale = Vector3.one;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public void ReleaseObject(T element)
        {
            element.gameObject.SetActive(false);
            FreeObjects.Enqueue(element);
        }

        public void ReleaseObjectAndResetParent(T element)
        {
            element.transform.SetParent(_parent);
            element.gameObject.SetActive(false);
            FreeObjects.Enqueue(element);
        }

        private T GetOrCreate()
        {
            var pooledObject = FreeObjects.Count > 0 ? FreeObjects.Dequeue() : CreateNewObject();
            if (pooledObject == null) return GetOrCreate();
            return pooledObject;
        }
        
        private T CreateNewObject()
        {
            _count++;
            var newObject = Object.Instantiate(_prefab, _parent).GetComponent<T>();
            newObject.name = $"{_elementName} {_count}";
            return newObject;
        }
    }
}