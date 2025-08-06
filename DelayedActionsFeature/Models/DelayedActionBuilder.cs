using System;
using System.Collections.Concurrent;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public partial class DelayedAction
    {
        public class Builder
        {
            private Builder() { }

            private static readonly ConcurrentQueue<Builder> BuildersPool = new();

            public static Builder CreateBuilder(float checkDelay, Action action)
            {
                if (!BuildersPool.TryDequeue(out var builder))
                {
                    builder = new Builder();
                }

                builder._delay = checkDelay;
                builder._action = action;
                return builder;
            }

            private Action _action;
            private float _delay;
            private float _timeout;
            private float _cycleDelay;
            private Func<bool> _validationFunc;
            private Func<bool> _conditionFunc;

            public Builder WithTimeout(float time)
            {
                _timeout = time;
                return this;
            }

            public Builder WithValidation(Func<bool> validationFunc)
            {
                _validationFunc = validationFunc;
                return this;
            }

            public Builder WithCondition(Func<bool> conditionFunc)
            {
                _conditionFunc = conditionFunc;
                return this;
            }

            public Builder WithCycle(float cycleDelay)
            {
                _cycleDelay = cycleDelay;
                return this;
            }

            private void Clear()
            {
                _action  = null;
                _delay = 0;
                _timeout = 0;
                _cycleDelay = 0;
                _validationFunc = null;
                _conditionFunc = null;
            }

            public Handle Run()
            {
                int id;
                
                lock (IdLock)
                {
                    id = _nextId++;
                }
                
                var handle = new Handle(id);
                
                CreateOperation(
                    id: id, 
                    timeoutDelay: _timeout,
                    checkDelay: _delay,
                    validationFunc: _validationFunc,
                    conditionFunc: _conditionFunc,
                    action: _action,
                    cycleDelay: _cycleDelay);
                
                Clear();
                BuildersPool.Enqueue(this);
                return handle;
            }
        }
    }
}