
using Exerussus._1Extensions.Loader;
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem
{
    public abstract class MonoSignalListener : MonoBehaviour
    {
        [SerializeField, HideInInspector] private SignalHandler signalHandler;
        public Signal Signal => signalHandler.Signal;

        protected virtual void OnValidate()
        {
            if (signalHandler == null) signalHandler =  ProjectLoader.Loader.GetAssetFromResourceByName<SignalHandler>();
        }
    }
    
    public abstract class MonoSignalListener<T> : MonoSignalListener where T : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T>(OnSignal);
        }

        protected abstract void OnSignal(T data);
    }
    
    public abstract class MonoSignalListener<T1, T2> : MonoSignalListener where T1 : struct where T2 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3> : MonoSignalListener where T1 : struct where T2 : struct where T3 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4> : MonoSignalListener where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5> : MonoSignalListener where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6> : MonoSignalListener 
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6, T7> : MonoSignalListener 
        where T1 : struct 
        where T2 : struct 
        where T3 : struct 
        where T4 : struct 
        where T5 : struct 
        where T6 : struct 
        where T7 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
            Signal.Subscribe<T7>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
            Signal.Unsubscribe<T7>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
        protected abstract void OnSignal(T7 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6, T7, T8> : MonoSignalListener 
        where T1 : struct 
        where T2 : struct 
        where T3 : struct 
        where T4 : struct 
        where T5 : struct 
        where T6 : struct 
        where T7 : struct
        where T8 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
            Signal.Subscribe<T7>(OnSignal);
            Signal.Subscribe<T8>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
            Signal.Unsubscribe<T7>(OnSignal);
            Signal.Unsubscribe<T8>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
        protected abstract void OnSignal(T7 data);
        protected abstract void OnSignal(T8 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6, T7, T8, T9> : MonoSignalListener 
        where T1 : struct 
        where T2 : struct 
        where T3 : struct 
        where T4 : struct 
        where T5 : struct 
        where T6 : struct 
        where T7 : struct
        where T8 : struct
        where T9 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
            Signal.Subscribe<T7>(OnSignal);
            Signal.Subscribe<T8>(OnSignal);
            Signal.Subscribe<T9>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
            Signal.Unsubscribe<T7>(OnSignal);
            Signal.Unsubscribe<T8>(OnSignal);
            Signal.Unsubscribe<T9>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
        protected abstract void OnSignal(T7 data);
        protected abstract void OnSignal(T8 data);
        protected abstract void OnSignal(T9 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : MonoSignalListener 
        where T1 : struct 
        where T2 : struct 
        where T3 : struct 
        where T4 : struct 
        where T5 : struct 
        where T6 : struct 
        where T7 : struct
        where T8 : struct
        where T9 : struct
        where T10 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
            Signal.Subscribe<T7>(OnSignal);
            Signal.Subscribe<T8>(OnSignal);
            Signal.Subscribe<T9>(OnSignal);
            Signal.Subscribe<T10>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
            Signal.Unsubscribe<T7>(OnSignal);
            Signal.Unsubscribe<T8>(OnSignal);
            Signal.Unsubscribe<T9>(OnSignal);
            Signal.Unsubscribe<T10>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
        protected abstract void OnSignal(T7 data);
        protected abstract void OnSignal(T8 data);
        protected abstract void OnSignal(T9 data);
        protected abstract void OnSignal(T10 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : MonoSignalListener 
        where T1 : struct 
        where T2 : struct 
        where T3 : struct 
        where T4 : struct 
        where T5 : struct 
        where T6 : struct 
        where T7 : struct
        where T8 : struct
        where T9 : struct
        where T10 : struct
        where T11 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
            Signal.Subscribe<T7>(OnSignal);
            Signal.Subscribe<T8>(OnSignal);
            Signal.Subscribe<T9>(OnSignal);
            Signal.Subscribe<T10>(OnSignal);
            Signal.Subscribe<T11>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
            Signal.Unsubscribe<T7>(OnSignal);
            Signal.Unsubscribe<T8>(OnSignal);
            Signal.Unsubscribe<T9>(OnSignal);
            Signal.Unsubscribe<T10>(OnSignal);
            Signal.Unsubscribe<T11>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
        protected abstract void OnSignal(T7 data);
        protected abstract void OnSignal(T8 data);
        protected abstract void OnSignal(T9 data);
        protected abstract void OnSignal(T10 data);
        protected abstract void OnSignal(T11 data);
    }
    
    public abstract class MonoSignalListener<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : MonoSignalListener 
        where T1 : struct 
        where T2 : struct 
        where T3 : struct 
        where T4 : struct 
        where T5 : struct 
        where T6 : struct 
        where T7 : struct
        where T8 : struct
        where T9 : struct
        where T10 : struct
        where T11 : struct
        where T12 : struct
    {
        protected virtual void OnEnable()
        {
            Signal.Subscribe<T1>(OnSignal);
            Signal.Subscribe<T2>(OnSignal);
            Signal.Subscribe<T3>(OnSignal);
            Signal.Subscribe<T4>(OnSignal);
            Signal.Subscribe<T5>(OnSignal);
            Signal.Subscribe<T6>(OnSignal);
            Signal.Subscribe<T7>(OnSignal);
            Signal.Subscribe<T8>(OnSignal);
            Signal.Subscribe<T9>(OnSignal);
            Signal.Subscribe<T10>(OnSignal);
            Signal.Subscribe<T11>(OnSignal);
            Signal.Subscribe<T12>(OnSignal);
        }

        protected virtual void OnDisable()
        {
            Signal.Unsubscribe<T1>(OnSignal);
            Signal.Unsubscribe<T2>(OnSignal);
            Signal.Unsubscribe<T3>(OnSignal);
            Signal.Unsubscribe<T4>(OnSignal);
            Signal.Unsubscribe<T5>(OnSignal);
            Signal.Unsubscribe<T6>(OnSignal);
            Signal.Unsubscribe<T7>(OnSignal);
            Signal.Unsubscribe<T8>(OnSignal);
            Signal.Unsubscribe<T9>(OnSignal);
            Signal.Unsubscribe<T10>(OnSignal);
            Signal.Unsubscribe<T11>(OnSignal);
            Signal.Unsubscribe<T12>(OnSignal);
        }

        protected abstract void OnSignal(T1 data);
        protected abstract void OnSignal(T2 data);
        protected abstract void OnSignal(T3 data);
        protected abstract void OnSignal(T4 data);
        protected abstract void OnSignal(T5 data);
        protected abstract void OnSignal(T6 data);
        protected abstract void OnSignal(T7 data);
        protected abstract void OnSignal(T8 data);
        protected abstract void OnSignal(T9 data);
        protected abstract void OnSignal(T10 data);
        protected abstract void OnSignal(T11 data);
        protected abstract void OnSignal(T12 data);
    }
}