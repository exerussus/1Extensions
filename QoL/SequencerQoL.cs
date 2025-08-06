using Exerussus._1Extensions.LoopFeature;

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
                    ExerussusLoopHelper.OnUpdate -= _actionSequencer.Update;
                    ExerussusLoopHelper.OnUpdate += _actionSequencer.Update;
                }
                _count++;
                return _actionSequencer.CreateSequenceCommander(_count);
            }
        }
    }
}