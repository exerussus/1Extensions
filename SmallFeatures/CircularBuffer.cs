using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Exerussus._1Extensions.SmallFeatures
{
    public struct CircularBuffer<T> : IEnumerable<T>
    {
        private T[] _buffer;
        public int Count { get; private set; }
        public int StartIndex { get; private set; }
        public int Capacity => _buffer.Length;

        public CircularBuffer(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            _buffer = new T[capacity];
            Count = 0;
            StartIndex = 0;
        }

        /// <summary> Добавляет элемент в конец. Если Массив полон - заменяет самый старый элемент (первый кандидат на Pop). </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T item)
        {
            var insertIndex = (StartIndex + Count) % Capacity;

            if (Count == Capacity)
            {
                _buffer[insertIndex] = item;
                StartIndex = (StartIndex + 1) % Capacity;
            }
            else
            {
                _buffer[insertIndex] = item;
                Count++;
            }
        }

        /// <summary> Удаляет первый элемент. Он же самый старый. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pop()
        {
            if (Count <= 0) return;
            StartIndex = (StartIndex + 1) % Capacity;
            Count--;
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)Count) throw new ArgumentOutOfRangeException(nameof(index));
                return ref _buffer[(StartIndex + index) % Capacity];
            }
        }

        /// <summary> Обнуляет счетчик. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Count = 0;
            StartIndex = 0;
        }
        
        public Enumerator GetEnumerator() => new Enumerator(ref this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(ref this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(ref this);

        public struct Enumerator : IEnumerator<T>
        {
            private readonly CircularBuffer<T> _buffer;
            private int _index;
            private T _current;

            internal Enumerator(ref CircularBuffer<T> buffer)
            {
                _buffer = buffer;
                _index = -1;
                _current = default!;
            }

            public bool MoveNext()
            {
                if (_index + 1 >= _buffer.Count) return false;
                _index++;
                _current = _buffer[_index];
                return true;
            }

            public T Current => _current;

            object IEnumerator.Current => Current!;

            public void Dispose() { }

            public void Reset() => _index = -1;
        }
    }
}
