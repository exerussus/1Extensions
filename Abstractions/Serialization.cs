namespace Exerussus._1Extensions.Abstractions
{
    public interface IOnDeserialized
    {
        public void OnDeserialized();
    }
    
    public interface IOnBeforeSerialize
    {
        public void OnDeserialized();
    }
    
    public interface IOnDeserialized<in T>
    {
        public void OnDeserialized(T reference);
    }
    
    public interface IOnBeforeSerialize<in T>
    {
        public void OnDeserialized(T reference);
    }
}