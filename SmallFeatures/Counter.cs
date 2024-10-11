namespace Plugins.Exerussus._1Extensions.SmallFeatures
{
    public class Counter
    {
        public Counter()
        {
            _default = 0;
            _value = 0;
        }

        public Counter(int startValue)
        {
            _default = startValue;
            _value = startValue;
        }

        private int _default;
        private int _value;

        public void Clear()
        {
            _value = _default;
        }
        
        public int GetNext()
        {
            _value++;
            return _value;
        }
    }
    
    public class ReversedCounter
    {
        public ReversedCounter()
        {
            _default = 0;
            _value = 0;
        }

        public ReversedCounter(int startValue)
        {
            _default = startValue;
            _value = startValue;
        }

        private int _default;
        private int _value;

        public void Clear()
        {
            _value = _default;
        }
        
        public int GetNext()
        {
            _value--;
            return _value;
        }
    }
}