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
}