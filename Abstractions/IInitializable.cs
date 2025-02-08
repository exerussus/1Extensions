using Exerussus._1Extensions.SmallFeatures;

namespace Exerussus._1Extensions.Abstractions
{
    public interface IInjectable
    {
        public void Inject(GameShare gameShare);
    }

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
    
    public interface IInitializable<in T>
    {
        public void Initialize(T reference);
    }

    public interface IBeforeInitializable<in T>
    {
        public void BeforeInitialize(T reference);
    }

    public interface IPostInitializable<in T>
    {
        public void PostInitialize(T reference);
    }
}