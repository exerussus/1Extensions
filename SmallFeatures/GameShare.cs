
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Exerussus._1Extensions.SmallFeatures
{
    [Serializable]
    public class GameShare
    {
#if UNITY_EDITOR
        [SerializeField] private List<string> sharedObjects = new ();
#endif
        private Dictionary<Type, DataPackByType> _sharedObjectsByType = new();
        private Dictionary<string, DataPackById> _sharedObjectsById = new();
        private Dictionary<Type, Dictionary<Type, DataPackByType>> _subObjects = new();

        [Preserve]
        private T InjectSharedObject<T>(Type type)
        {
            return GetSharedObject<T>(type);
        }
        
        [Preserve]
        private T InjectSharedSubTypeObject<T>(Type mainType, Type subType)
        {
            return GetSharedObject<T>(mainType, subType);
        }
        
        [Preserve]
        public T GetSharedObject<T>()
        {
            var classPack = _sharedObjectsByType[typeof(T)];
            var sharedObject = classPack.Object;
            return (T)sharedObject;
        }
        
        [Preserve]
        public T2 GetSharedObject<T1, T2>()
        {
            var classPack = _subObjects[typeof(T1)][typeof(T2)];
            var sharedObject = classPack.Object;
            return (T2)sharedObject;
        }
        
        [Preserve]
        public T GetSharedObject<T>(string id)
        {
            var classPack = _sharedObjectsById[id];
            var sharedObject = classPack.Object;
            return (T)sharedObject;
        }
        
        [Preserve]
        public void GetSharedObject<T>(ref T sharedObject)
        {
            var classPack = _sharedObjectsByType[typeof(T)];
            sharedObject = (T)classPack.Object;
        }
        
        [Preserve]
        public T GetSharedObject<T>(Type type)
        {
            var classPack = _sharedObjectsByType[type];
            return (T)classPack.Object;
        }
        
        [Preserve]
        public void GetSharedObject<T1, T2>(ref T2 sharedObject)
        {
            var classPack = _subObjects[typeof(T1)][typeof(T2)];
            sharedObject = (T2)classPack.Object;
        }
        
        [Preserve]
        public T GetSharedObject<T>(Type mainType, Type subType)
        {
            var classPack = _subObjects[mainType][subType];
            return (T)classPack.Object;
        }
        
        [Preserve]
        public void GetSharedObject<T>(string id, ref T sharedObject)
        {
            var classPack = _sharedObjectsById[id];
            sharedObject = (T)classPack.Object;
        }

        [Preserve]
        public void AddSharedObject<T>(Type type, T sharedObject)
        {
#if UNITY_EDITOR
            sharedObjects.Add(type.Name);
#endif
            _sharedObjectsByType[type] = new DataPackByType(type, sharedObject);
        }

        [Preserve]
        public void AddSharedObject<T>(string id, T sharedObject)
        {
#if UNITY_EDITOR
            sharedObjects.Add(id);
#endif
            _sharedObjectsById[id] = new DataPackById(id, sharedObject);
        }

        [Preserve]
        public void AddSharedObject<T>(T sharedObject)
        {
            var type = sharedObject.GetType();
#if UNITY_EDITOR
            sharedObjects.Add(type.Name);
#endif
            _sharedObjectsByType[type] = new DataPackByType(type, sharedObject);
        }

        [Preserve]
        public void AddSharedObject<T1, T2>(T2 sharedObject)
        {
            var type1 = typeof(T1);
            var type2 = sharedObject.GetType();
#if UNITY_EDITOR
            sharedObjects.Add($"{type1.Name}.{type2.Name}");
#endif
            if (!_subObjects.ContainsKey(type1)) _subObjects[type1] = new Dictionary<Type, DataPackByType>();
            _subObjects[type1][type2] = new DataPackByType(type2, sharedObject);
        }

        [Preserve]
        public void AddSharedObject(Type main, Type sub, object sharedObject)
        {
#if UNITY_EDITOR
            sharedObjects.Add($"{main.Name}.{sub.Name}");
#endif
            if (!_subObjects.ContainsKey(main)) _subObjects[main] = new Dictionary<Type, DataPackByType>();
            _subObjects[main][sub] = new DataPackByType(sub, sharedObject);
        }
    }
    
    [Serializable]
    public class DataPackByType
    {
        public DataPackByType(Type type, object sharedObject)
        {
            _object = sharedObject;
            _type = type;
            name = _type.Name;
        }

        [SerializeField] private string name;
        private Type _type;
        private object _object;
        
        public string Name => name; 
        public object Object => _object;
    }
    
    [Serializable]
    public class DataPackById
    {
        public DataPackById(string id, object sharedObject)
        {
            _object = sharedObject;
            _id = id;
            name = id;
        }

        [SerializeField] private string name;
        private string _id;
        private object _object;
        
        public string Name => name; 
        public object Object => _object;
    }
}