using System;
using System.Collections.Generic;
using Exerussus._1Extensions.Abstractions;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public abstract class Library<T> : ScriptableObject, ILibrary
        where T : ILibraryItem, new()
    {
        protected HashSet<string> _hashSet;
        protected Dictionary<string, T> _itemByTypeID;
        protected Dictionary<long, T> _itemByID;
        public abstract List<T> Items { get; set; }
        public bool IsInitialized { get; protected set; }
        protected abstract string DefaultValue { get; }
        
         /// <summary> Use it runtime only. </summary>
        public virtual bool TryGet(string id, out T val)
        {
            return _itemByTypeID.TryGetValue(id, out val);
        }
         
         /// <summary> Use it runtime only. </summary>
        public virtual bool TryGet(long id, out T val)
        {
            return _itemByID.TryGetValue(id, out val);
        }
         
         /// <summary> Use it runtime only. </summary>
        public virtual T GetOrDefault(string id)
        {
            if (TryGet(id, out var pack)) return pack;
            Debug.LogWarning($"Libraries | {GetType().Name} | Не найден {id}, по умолчанию {DefaultValue}");
            return Get(DefaultValue);
        }
         /// <summary> Use it runtime only. </summary>
        public virtual T GetOrDefault(long id)
        {
            if (TryGet(id, out var pack)) return pack;
            Debug.LogWarning($"Libraries | {GetType().Name} | Не найден {id}, по умолчанию {DefaultValue}");
            return Get(DefaultValue);
        }
         
        /// <summary> Возвращает значение, если найден ID, либо возвращает дефолтное значение. </summary>
        public virtual T GetOrDefault(string id, string defaultValue)
        {
            if (TryGet(id, out var pack)) return pack;
            return Get(defaultValue);
        }
         
        /// <summary> Возвращает значение, если найден ID, либо возвращает дефолтное значение. </summary>
        public virtual T GetOrDefault(long id, long defaultValue)
        {
            if (TryGet(id, out var pack)) return pack;
            return Get(defaultValue);
        }
         
        /// <summary> Use it runtime only. </summary>
        public virtual T Get(string id)
        {
            return _itemByTypeID[id];
        }
         
        /// <summary> Use it runtime only. </summary>
        public virtual T Get(long id)
        {
            return _itemByID[id];
        }

        public virtual bool ContainsID(string itemId)
        {
            return _itemByTypeID.ContainsKey(itemId);
        }

        public virtual bool ContainsID(long itemId)
        {
            return _itemByID.ContainsKey(itemId);
        }

        public void Clear()
        {
            IsInitialized = false;
            _itemByTypeID.Clear();
            _itemByID.Clear();
        }

        /// <summary> Use it on validation only. </summary>
        public virtual T GetByIDIterations(string id)
        {
            Initialize();
            if (_itemByTypeID.ContainsKey(id))
            {
                return _itemByTypeID[id];
            }

            throw new Exception($"Cannot find library item in library : <<{GetType().Name}>> by index : <<{id}>>.");
        }
        
        public void UpdateDictionary()
        {
            _itemByTypeID = new();
            _itemByID = new();
            foreach (var gameItem in Items)
            {
                _itemByTypeID[gameItem.TypeId] = gameItem;
                var stableLong = gameItem.TypeId.GetStableLongId();
                _itemByID[stableLong] = gameItem;
            }
        }

        public virtual void Initialize()
        {
            #if !UNITY_EDITOR
                if (IsInitialized) return;
            #endif
            
            _itemByTypeID = new Dictionary<string, T>();
            _itemByID = new Dictionary<long, T>();
            _hashSet = new HashSet<string>();

            foreach (var item in Items)
            {
                item.Initialize();
                _itemByTypeID[item.TypeId] = item;
                _itemByID[item.Id] = item;
                _hashSet.Add(item.TypeId);
            }

            IsInitialized = true;
        }

        protected virtual void OnValidation()
        {
        }

        private void OnValidate()
        {
            OnValidation();
            foreach (var item in Items) item.OnValidation();
        }
    }
    
    public interface ILibraryItem
    {
        public abstract string TypeId { get; set; }
        public long Id { get; protected set; }

        public void Initialize()
        {
            Id = TypeId.GetStableLongId();
            OnInitialize();
        }
        
        public virtual void OnInitialize() {}
        public virtual void OnValidation() {}
    }

    public interface ILibrary : IInitializable
    {
        public void UpdateDictionary();
        public bool ContainsID(string itemId);
        public void Clear();
    }
}