using Cysharp.Threading.Tasks;
using Exerussus._1Extensions.SmallFeatures;

namespace Exerussus._1Extensions.Abstractions
{
    public interface IBeforeInitializable
    {
        public void BeforeInitialize();
    }

    public interface IPostInitializable
    {
        public void PostInitialize();
    }

    public interface IInitializable
    {
        public void Initialize();
    }

    public interface IInitializableAsync
    {
        public UniTask Initialize();
    }

    public interface IInitializable<in T>
    {
        public void Initialize(T reference);
    }

    public interface IInitializableAsync<in T>
    {
        public UniTask Initialize(T reference);
    }

    public interface IBeforeInitializable<in T>
    {
        public void BeforeInitialize(T reference);
    }

    public interface IPostInitializable<in T>
    {
        public void PostInitialize(T reference);
    }
    
    public interface IDeinitializable
    {
        public void Deinitialize();
    }
}