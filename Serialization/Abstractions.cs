using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    public interface IPersistable
    {
        public void Save();
        public void Load();
    }
    
    public interface IPersistable<in TSaveRef, in TLoadRef>
    {
        public void Save(TSaveRef reference);
        public void Load(TLoadRef reference);
    }

    public abstract class Persistable<T> : IPersistable
    {
        public abstract string AssetFolder { get; }
        
        public void Save()
        {
            AssetSerializer.SavePersistent(this, AssetFolder);
        }

        public void Load()
        {
            var (result, loaded) = AssetSerializer.LoadPersistent<T>(AssetFolder);
            if (result) OnLoad(loaded);
            else
            {
                Debug.LogWarning("Failed to load. Creating and saving new.");
                Save();
            }
        }

        public abstract void OnLoad(T loaded);
    }
}