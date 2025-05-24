using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public class ActionSequencer
    {
        private readonly Dictionary<int, SequenceAction> _dict = new();
        private int _freeId;
        
        public int CreateSequence(float delay)
        {
            _freeId++;
            _dict.Add(_freeId, new SequenceAction(delay));
            return _freeId;
        }
        
        public void AddToSequence(int id, Action action)
        {
            _dict[id].Queue.Enqueue(action);
        }

        public bool IsDone(int id)
        {
            return _dict[id].Queue.Count == 0;
        }

        public void ClearSequence(int id)
        {
            if (_dict.TryGetValue(id, out var sequenceAction))
            {
                sequenceAction.Queue.Clear();
            }
        }

        public void ClearAllSequences()
        {
            foreach (var sequenceAction in _dict.Values)
            {
                sequenceAction.Queue.Clear();
            }
        }

        public void Update()
        {
            foreach (var sequenceAction in _dict.Values)
            {
                if (sequenceAction.Queue.Count != 0 && Time.time > sequenceAction.NextUpdateTime)
                {
                    sequenceAction.NextUpdateTime = Time.time + sequenceAction.Delay;
                    sequenceAction.Queue.Dequeue().Invoke();
                }
            }
        }

        public void UpdateCurrent(int id)
        {
            var sequenceAction = _dict[id];
            
            if (sequenceAction.Queue.Count != 0 && Time.time > sequenceAction.NextUpdateTime)
            {
                sequenceAction.NextUpdateTime = Time.time + sequenceAction.Delay;
                sequenceAction.Queue.Dequeue().Invoke();
            }
        }
    }

    public static class ActionSequencerExtensions
    {
        public static SequenceCommander CreateSequenceCommander(this ActionSequencer sequencer, float delay)
        {
            return new SequenceCommander(sequencer.CreateSequence(delay), sequencer);
        }
    }

    public class SequenceCommander
    {
        public SequenceCommander(int id, ActionSequencer sequencer)
        {
            _id = id;
            _sequencer = sequencer;
        }

        private readonly int _id;
        private readonly ActionSequencer _sequencer;
        
        public void AddToSequence(Action action) => _sequencer.AddToSequence(_id, action);
        public bool IsDone() => _sequencer.IsDone(_id);
        public void ClearSequence() => _sequencer.ClearSequence(_id);
        public void ClearAllSequences() => _sequencer.ClearAllSequences();
        public void Update() => _sequencer.UpdateCurrent(_id);
        public void Dispose() => _sequencer.ClearSequence(_id);
    }

    public class SequenceAction
    {
        public SequenceAction(float delay)
        {
            Delay = delay;
        }

        public readonly float Delay;
        public readonly Queue<Action> Queue = new();
        public float NextUpdateTime;
    }
}