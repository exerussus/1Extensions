namespace Exerussus._1Extensions.Abstractions
{
    public interface IValidatable
    {
        public bool IsValid();
    }
    
    public interface IValidatable<in T>
    {
        public bool IsValid(T reference);
    }
}