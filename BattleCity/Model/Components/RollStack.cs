using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Model.Components
{
    class RollStack<T>
    {
        int count = 0;
        int top = -1;
        T[] arr;

        public RollStack(int length)
        {
            arr = new T[length];
        }

        public bool IsEmpty() => count == 0;

        public void Push(T elem)
        {
            top = (top + 1) % arr.Length;
            arr[top] = elem;
            count = (count < arr.Length) ? count+1 : count;
        }

        public T Pop()
        {
            T elem = arr[top];
            top = (top + arr.Length - 1) % arr.Length;
            count--;
            return elem;
        }

        public T Peek() => arr[top];

        public void MakeEmpty() => count = 0;
    }
}
