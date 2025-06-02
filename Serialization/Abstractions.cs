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
}