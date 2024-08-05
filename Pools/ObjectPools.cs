using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.Pools
{
    public class ObjectPool<T> where T : Component
    {
        public ObjectPool(GameObject prefab, int count = 1)
        {
            _prefab = prefab;
            InitPrefab(prefab, count);
        }
        
        private GameObject _prefab;
        private const int DefaultObjectCount = 5;
        private Queue<T> FreeObjects { get; } = new();

        public void InitPrefab(GameObject prefab, int count = DefaultObjectCount)
        {
            _prefab = prefab;
            for (int i = 0; i < count; i++)
            {
                var element = CreateNewObject();
                element.gameObject.SetActive(false);
                FreeObjects.Enqueue(element);
            }
        }

        public T GetObject(Vector3 position, Quaternion rotation)
        {
            var pooledObject = FreeObjects.Count > 0 ? FreeObjects.Dequeue() : CreateNewObject();
            pooledObject.transform.SetPositionAndRotation(position, rotation);
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public T GetObject(Vector3 position, Vector3 scale)
        {
            var pooledObject = FreeObjects.Count > 0 ? FreeObjects.Dequeue() : CreateNewObject();
            pooledObject.transform.SetPositionAndRotation(position, Quaternion.identity);
            pooledObject.gameObject.transform.localScale = scale;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        public void ReleaseObject(T element)
        {
            element.gameObject.SetActive(false);
            FreeObjects.Enqueue(element);
        }

        private T CreateNewObject()
        {
            return Object.Instantiate(_prefab).GetComponent<T>();
        }

        public T GetObject(Vector3 position, Vector3 scale, Vector3 rotation)
        {
            var pooledObject = FreeObjects.Count > 0 ? FreeObjects.Dequeue() : CreateNewObject();
            pooledObject.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
            pooledObject.gameObject.transform.localScale = scale;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }
    }
}