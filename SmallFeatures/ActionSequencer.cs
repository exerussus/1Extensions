using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public class ActionSequencer
    {
        private readonly Dictionary<int, Sequence> _dict = new();
        private int _freeId;
        
        public int CreateSequence(float delay)
        {
            _freeId++;
            _dict.Add(_freeId, new Sequence(delay));
            return _freeId;
        }

        public void ChangeBaseDelay(int id, float delay)
        {
            _dict[id].BaseDelay = delay;
        }

        public void ChangeCurrentDelay(int id, float delay)
        {
            var sequence = _dict[id];
            if (sequence.Queue.Count == 0) return;
            sequence.Queue[0].Delay = delay;
        }
        
        public SequenceAction AddToSequence(int id, Action action)
        {
            var sequence = _dict[id];
            var sequenceAction = new SequenceAction(action, sequence.BaseDelay);
            sequence.Queue.Add(sequenceAction);
            return sequenceAction;
        }
        
        public SequenceAction AddToSequence(int id, float delay, Action action)
        {
            var sequence = _dict[id];
            var sequenceAction = new SequenceAction(action, delay);
            sequence.Queue.Add(sequenceAction);
            return sequenceAction;
        }

        public void SetBlock(int id, bool isBlock)
        {
            var sequence = _dict[id];
            sequence.IsBlock = isBlock;
        }

        public bool IsDone(int id)
        {
            return _dict[id].Queue.Count == 0;
        }

        public bool IsBlock(int id)
        {
            var sequence = _dict[id];
            if (sequence.Queue.Count == 0) return false;
            return sequence.IsBlock;
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
            foreach (var sequence in _dict.Values)
            {
                if (sequence.IsBlock) continue;
                if (sequence.Queue.Count != 0 && Time.time > sequence.NextUpdateTime)
                {
                    var sequenceAction = sequence.Queue[0];
                    sequence.Queue.RemoveAt(0);
                    sequenceAction.Action.Invoke();
                    sequence.NextUpdateTime = Time.time + sequenceAction.Delay;
                }
            }
        }

        public void UpdateCurrent(int id)
        {
            var sequence = _dict[id];
            if (sequence.IsBlock) return;
            
            if (sequence.Queue.Count != 0 && Time.time > sequence.NextUpdateTime)
            {
                var sequenceAction = sequence.Queue[0];
                sequence.Queue.RemoveAt(0);
                sequenceAction.Action.Invoke();
                sequence.NextUpdateTime = Time.time + sequenceAction.Delay;
            }
        }
    }

    public static class ActionSequencerExtensions
    {
        public static SequenceCommander CreateSequenceCommander(this ActionSequencer sequencer, float baseDelay)
        {
            return new SequenceCommander(sequencer.CreateSequence(baseDelay), sequencer);
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
        
        public SequenceAction AddToSequence(Action action)
        {
            return _sequencer.AddToSequence(_id, action);
        }

        public SequenceAction AddToSequence(float delay, Action action)
        {
            return _sequencer.AddToSequence(_id, delay, action);
        }

        public SequenceCommander ChangeBaseDelay(float delay)
        {
            _sequencer.ChangeBaseDelay(_id, delay);
            return this;
        }

        public bool IsDone() => _sequencer.IsDone(_id);
        public SequenceCommander ClearSequence()
        {
            _sequencer.ClearSequence(_id);
            return this;
        }

        public SequenceCommander ClearAllSequences()
        {
            _sequencer.ClearAllSequences();
            return this;
        }
        
        public SequenceCommander ChangeCurrentDelay(float delay)
        {
            _sequencer.ChangeCurrentDelay(_id, delay);
            return this;
        }

        public SequenceCommander SetBlock(bool isBlock)
        {
            _sequencer.SetBlock(_id, isBlock);
            return this;
        }

        public void Update()
        {
            _sequencer.UpdateCurrent(_id);
        }

        public bool IsBlock()
        {
            return _sequencer.IsBlock(_id);
        }
    }

    public class Sequence
    {
        public Sequence(float baseDelay)
        {
            BaseDelay = baseDelay;
        }

        public readonly List<SequenceAction> Queue = new();
        public float BaseDelay;
        public float NextUpdateTime;
        public bool IsBlock;
    }

    public class SequenceAction
    {
        public SequenceAction(Action action)
        {
            Action = action;
            Delay = -1;
        }

        public SequenceAction(Action action, float delay)
        {
            Action = action;
            Delay = delay;
        }

        public Action Action;
        public float Delay;
    }
}