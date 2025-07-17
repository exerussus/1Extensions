namespace Exerussus._1Extensions.SmallFeatures
{
    public static class SequencerQoL
    {
        private static ActionSequencer _actionSequencer;
        private static int _count;
        private static object _lock = new();
        
        public static SequenceCommander CreateSequence()
        {
            lock (_lock)
            {
                if (_actionSequencer == null)
                {
                    _actionSequencer = new();
                    _count = 0;
                    ExerussusCore.AddOnUpdate(UpdateSub);
                }
                _count++;
                return _actionSequencer.CreateSequenceCommander(_count);
            }
        }

        private static void UpdateSub()
        {
            _actionSequencer.Update();
        }
    }
}