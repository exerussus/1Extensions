using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    /// <summary> Даёт возможность создать последовательность, к которой можно добавлять события для их последовательного выполнения. </summary>
    public class ActionSequencer
    {
        private readonly Dictionary<int, Sequence> _dict = new();
        private int _freeId;
        
        /// <summary> Создает новую последовательность. </summary>
        /// <param name="delay">Базовая задержка между событиями.</param>
        /// <returns>Уникальный ID последовательности для обращения к ней.</returns>
        public int CreateSequence(float delay)
        {
            _freeId++;
            _dict.Add(_freeId, new Sequence(delay));
            return _freeId;
        }

        /// <summary> Изменяет базовую задержку между событиями. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        /// <param name="delay">Задержка между событиями.</param>
        public void ChangeBaseDelay(int id, float delay)
        {
            _dict[id].BaseDelay = delay;
        }
        
        /// <summary> Добавляет событие в последовательность с базовой задержкой. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        /// <param name="action">Событие.</param>
        public void AddToSequence(int id, Action action)
        {
            var sequence = _dict[id];
            sequence.Enqueue(new SequenceAction(action, sequence.BaseDelay));
        }

        /// <summary> Добавляет событие в последовательность с указанной задержкой. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        /// <param name="delay">Задержка после данного события.</param>
        /// <param name="action">Событие.</param>
        public void AddToSequence(int id, float delay, Action action)
        {
            var sequence = _dict[id];
            sequence.Enqueue(new SequenceAction(action, delay));
        }

        /// <summary> Блокирует, или активирует процесс выполнения событий. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        /// <param name="isBlock">Если true, блокирует процесс выполнения.</param>
        public void SetBlock(int id, bool isBlock)
        {
            var sequence = _dict[id];
            sequence.IsBlock = isBlock;
        }

        /// <summary> Проверяет, закончилась ли последовательность. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        /// <returns>True, если все события выполнены.</returns>
        public bool IsDone(int id)
        {
            return _dict[id].Count == 0;
        }

        /// <summary> Проверка на блокировку. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        /// <returns>True, если последовательность заблокирована.</returns>
        public bool IsBlock(int id)
        {
            var sequence = _dict[id];
            if (sequence.Count == 0) return false;
            return sequence.IsBlock;
        }

        /// <summary> Очищает конкретную последовательность от событий. </summary>
        /// <param name="id">Уникальный ID последовательности выданный после её создания.</param>
        public void ClearSequence(int id)
        {
            if (_dict.TryGetValue(id, out var sequence)) sequence.Clear();
        }

        /// <summary> Очищает все последовательности от событий. </summary>
        public void ClearAllSequences()
        {
            foreach (var sequence in _dict.Values) sequence.Clear();
        }

        /// <summary> Вызывает глобальное обновление всех последовательностей. Вызов может происходить сколько угодно раз. </summary>
        public void Update()
        {
            var timeNow = Time.time;
            
            foreach (var sequence in _dict.Values)
            {
                if (sequence.IsBlock) continue;
                if (timeNow < sequence.NextUpdateTime) continue;
                if (sequence.Count == 0) continue;
                
                var action = sequence.Dequeue();
                action.Action.Invoke();
                sequence.NextUpdateTime = timeNow + action.Delay;
            }
        }

        /// <summary> Вызывает обновление конкретной последовательности. Вызов может происходить сколько угодно раз. </summary>
        public void UpdateCurrent(int id)
        {
            var sequence = _dict[id];
            
            if (sequence.IsBlock) return;
            
            var timeNow = Time.time;
            
            if (timeNow < sequence.NextUpdateTime) return;
            if (sequence.Count == 0) return;
            
            var action = sequence.Dequeue();
            action.Action.Invoke();
            sequence.NextUpdateTime = timeNow + action.Delay;
        }
    }

    public static class ActionSequencerExtensions
    {
        public static SequenceCommander CreateSequenceCommander(this ActionSequencer sequencer, float baseDelay)
        {
            return new SequenceCommander(sequencer.CreateSequence(baseDelay), sequencer);
        }
    }

    /// <summary> Класс, предоставляющий команды для управления последовательностью в стиле Fluent API. </summary>
    public class SequenceCommander
    {
        public SequenceCommander(int id, ActionSequencer sequencer)
        {
            _id = id;
            _sequencer = sequencer;
        }

        private readonly int _id;
        private readonly ActionSequencer _sequencer;
        
        
        /// <summary> Добавляет событие в последовательность с базовой задержкой. </summary>
        /// <param name="action">Событие.</param>
        public SequenceCommander AddToSequence(Action action)
        {
            _sequencer.AddToSequence(_id, action);
            return this;
        }

        
        /// <summary> Добавляет событие в последовательность с базовой задержкой. </summary>
        /// <param name="delay">Задержка после данного события.</param>
        /// <param name="action">Событие.</param>
        public SequenceCommander AddToSequence(float delay, Action action)
        {
            _sequencer.AddToSequence(_id, delay, action);
            return this;
        }


        /// <summary> Изменяет базовую задержку между событиями. </summary>
        /// <param name="delay">Задержка между событиями.</param>
        public SequenceCommander ChangeBaseDelay(float delay)
        {
            _sequencer.ChangeBaseDelay(_id, delay);
            return this;
        }

        /// <summary> Проверяет, закончилась ли последовательность. </summary>
        /// <returns>True, если все события выполнены.</returns>
        public bool IsDone() => _sequencer.IsDone(_id);
        
        /// <summary> Очищает конкретную последовательность от событий. </summary>
        public SequenceCommander ClearSequence()
        {
            _sequencer.ClearSequence(_id);
            return this;
        }

        /// <summary> Блокирует, или активирует процесс выполнения событий. </summary>
        /// <param name="isBlock">Если true, блокирует процесс выполнения.</param>
        public SequenceCommander SetBlock(bool isBlock)
        {
            _sequencer.SetBlock(_id, isBlock);
            return this;
        }

        /// <summary> Вызывает обновление последовательности. Вызов может происходить сколько угодно раз. </summary>
        public void Update()
        {
            _sequencer.UpdateCurrent(_id);
        }

        /// <summary> Проверка на блокировку. </summary>
        /// <returns>True, если последовательность заблокирована.</returns>
        public bool IsBlock()
        {
            return _sequencer.IsBlock(_id);
        }
    }

    public class Sequence
    {
        public Sequence(float baseDelay, int capacity = 16)
        {
            BaseDelay = baseDelay;
            _actions = new SequenceAction[capacity];
        }

        public float BaseDelay;
        public float NextUpdateTime;
        public bool IsBlock;

        private SequenceAction[] _actions;
        private int _start;
        private int _end;
        private int _count;

        public int Count => _count;

        public void Enqueue(SequenceAction action)
        {
            if (_count == _actions.Length)
                Expand();

            _actions[_end] = action;
            _end = (_end + 1) % _actions.Length;
            _count++;
        }

        public SequenceAction Dequeue()
        {
            if (_count == 0) throw new InvalidOperationException("Empty sequence");
            var action = _actions[_start];
            _start = (_start + 1) % _actions.Length;
            _count--;
            return action;
        }

        public SequenceAction Peek()
        {
            if (_count == 0) throw new InvalidOperationException("Empty sequence");
            return _actions[_start];
        }

        public void Clear()
        {
            _start = _end = _count = 0;
            for (int i = 0; i < _actions.Length; i++) _actions[i] = default;
        }

        private void Expand()
        {
            var newCapacity = _actions.Length + 4;
            var newArray = new SequenceAction[newCapacity];
            for (int i = 0; i < _count; i++)
            {
                newArray[i] = _actions[(_start + i) % _actions.Length];
            }

            _actions = newArray;
            _start = 0;
            _end = _count;
        }
    }

    public struct SequenceAction
    {
        public SequenceAction(Action action, float delay)
        {
            Action = action;
            Delay = delay;
        }

        public readonly Action Action;
        public readonly float Delay;
    }
}