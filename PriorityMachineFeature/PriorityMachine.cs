
using System;
using System.Collections.Generic;
using Exerussus._1Extensions.Scripts.Extensions;

namespace Exerussus.PriorityMachineFeature
{
    [Serializable]
    public class PriorityMachine<TEntity, TSharedData>
    {
        public PriorityMachine(
            TEntity entityId, 
            TSharedData sharedData, 
            string defaultState, 
            Func<string, PriorityState<TEntity, TSharedData>> gettingState,
            Action<TEntity, TSharedData> onCreateContext,
            Action<TEntity, TSharedData> onClearContext
            )
        {
            _currentState = defaultState;
            _entityId = entityId;
            _sharedData = sharedData;
            _gettingState = gettingState;
            _onCreateContext = onCreateContext;
            _onClearContext = onClearContext;
            
            AddState(_gettingState.Invoke(_currentState));
        }

        private string _currentState;
        private TEntity _entityId;
        private TSharedData _sharedData;
        private Func<string, PriorityState<TEntity, TSharedData>> _gettingState;
        private Action<TEntity, TSharedData> _onCreateContext;
        private Action<TEntity, TSharedData> _onClearContext;
        private List<PriorityState<TEntity, TSharedData>> _states = new();
        private Dictionary<string, PriorityState<TEntity, TSharedData>> _statesDict = new();
        private Dictionary<string, int> _priorities = new();

        public string CurrentState => _currentState;
        
        public void Update()
        {
            var morePriorityState = _currentState;
            var maxPriority = int.MinValue;
            
            _onCreateContext.Invoke(_entityId, _sharedData);
            
            foreach (var state in _states)
            {
                var priority = state.CalculatePriority.Invoke(_entityId, _sharedData, _priorities[state.Id]);
                _priorities[state.Id] = priority;
                if (maxPriority < priority)
                {
                    maxPriority = priority;
                    morePriorityState = state.Id;
                }
            }

            if (_currentState != morePriorityState)
            {
                _statesDict[_currentState].OnExit.Invoke(_entityId, _sharedData);
                _statesDict[morePriorityState].OnEnter.Invoke(_entityId, _sharedData);
                _currentState = morePriorityState;
            }
            else _statesDict[_currentState].OnUpdate.Invoke(_entityId, _sharedData);
            
            _onClearContext.Invoke(_entityId, _sharedData);
        }

        public bool HasState(string stateId)
        {
            return _statesDict.ContainsKey(stateId);
        }
        
        private void AddState(PriorityState<TEntity, TSharedData> state)
        {
            _states.Add(state);
            _statesDict[state.Id] = state;
            state.OnCreate.Invoke(_entityId, _sharedData);
        }
        
        private void RemoveState(string stateId)
        {
            if (!_statesDict.TryPop(stateId, out var state)) return;
            _states.Add(state);
            state.OnRemove.Invoke(_entityId, _sharedData);
        }
    }
}
