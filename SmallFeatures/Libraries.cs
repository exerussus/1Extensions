
using System.Collections.Generic;
using Exerussus._1Extensions.Abstractions;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public abstract class Library<T> : ScriptableObject, ILibrary
        where T : ILibraryItem, new()
    {
        private readonly Dictionary<string, ItemWrapper> _itemByTypeID = new();
        private readonly Dictionary<long, ItemWrapper> _itemByID = new();
        public abstract IReadOnlyList<T> Items {  get; protected set; }
        public bool IsInitialized { get; protected set; }
        
         /// <summary> Use it runtime only. </summary>
        public virtual bool TryGet(string id, out T value)
        {
            if (_itemByTypeID.TryGetValue(id, out var wrapper))
            {
                value = wrapper.Item;
                return true;
            }
            
            value = default;
            return false;
        }
         
         /// <summary> Use it runtime only. </summary>
        public virtual bool TryGet(long id, out T value)
        {
            if (_itemByID.TryGetValue(id, out var wrapper))
            {
                value = wrapper.Item;
                return true;
            }
            
            value = default;
            return false;
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
            return _itemByTypeID[id].Item;
        }
         
        /// <summary> Use it runtime only. </summary>
        public virtual T Get(long id)
        {
            return _itemByID[id].Item;
        }

        public virtual bool ContainsID(string itemId)
        {
            return _itemByTypeID.ContainsKey(itemId);
        }

        public virtual bool ContainsID(long itemId)
        {
            return _itemByID.ContainsKey(itemId);
        }

        public virtual List<string> GetAllTypeIds()
        {
            Initialize();
            return new List<string>(_itemByTypeID.Keys);
        }

        public void Clear()
        {
            IsInitialized = false;
            _itemByTypeID.Clear();
            _itemByID.Clear();
        }

        /// <summary> Use it on validation only. </summary>
        public virtual bool TryGetByIDIterations(string id, out T value)
        {
            Initialize();
            if (_itemByTypeID.ContainsKey(id))
            {
                value = _itemByTypeID[id].Item;
                return true;
            }
            
            value = default;
            return false;
        }
        
        public void UpdateDictionary()
        {
            _itemByTypeID.Clear();
            _itemByID.Clear();
            
            foreach (var gameItem in Items)
            {
                if (_itemByTypeID.ContainsKey(gameItem.TypeId))
                {
                    Debug.LogError($"Library item with type id : <<{gameItem.TypeId}>> already exists in library : <<{GetType().Name}>>.", this);
                    continue;
                }
                var wrapper = new ItemWrapper(gameItem);
                _itemByTypeID[wrapper.Item.TypeId] = wrapper;
                _itemByID[wrapper.Id] = wrapper;
            }
        }

        public virtual void Initialize()
        {
            if (IsInitialized) return;
            
            UpdateDictionary();
            
            foreach (var itemWrapper in _itemByTypeID.Values) itemWrapper.Item.Initialize();
            
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

        private class ItemWrapper
        {
            public ItemWrapper(T item)
            {
                Item = item;
                Id = item.TypeId.GetStableLongId();
            }

            public long Id;
            public T Item;
        }
    }
    
    public interface ILibraryItem
    {
        public string TypeId { get; }

        public void Initialize()
        {
            if (string.IsNullOrEmpty(TypeId))
            {
                Debug.LogError($"LibraryItem | {GetType().Name} | TypeId is empty");
                return;
            }

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