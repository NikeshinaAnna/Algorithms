using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public class SortByCartesianTree<T> : ISort<T>
    {
        private Tree<T> buffer;
        private int length;
        public T[] arr;
        private T[] inputArr;
        public T[] outputArr { get; set; }
        private IComparer<T> _comparer;
        public SortByCartesianTree(T[] array, IComparer<T> comparer)
        {
            length = array.Length;
            arr = new T[length];
            outputArr = new T[length];
            inputArr = new T[length];
            Array.Copy(array, inputArr, length);
            _comparer = comparer;
        }
        public T[] Sort()
        {
            Array.Copy(inputArr, arr, length);
            //формируем дерево общее
            buffer = new Tree<T>(0, arr[0], null, null, _comparer);
            for (int i = 1; i < length; i++)
            {
                Tree<T> newTree = new Tree<T>(i, arr[i], null, null, _comparer);
                buffer = buffer.Merge(buffer, newTree);
            }
            for (int i = 0; i < length; i++)
            {
                outputArr[length - i - 1] = buffer.y;
                if (i < length - 1)
                {
                    buffer = buffer.Merge(buffer.Left, buffer.Right);
                }
            }
            return outputArr;
        }
    }
    public class Tree<T>
    {
        public int x;
        public T y;
        private IComparer<T> _comparer;
        public Tree<T> Left;
        public Tree<T> Right;

        public Tree(int x, T y, Tree<T> left, Tree<T> right, IComparer<T> comparer)
        {
            this.x = x;
            this.y = y;
            this.Left = left;
            this.Right = right;
            _comparer = comparer;
        }
        public Tree<T> Merge(Tree<T> L, Tree<T> R)
        {
            if (L == null) return R;
            if (R == null) return L;

            if (_comparer.Compare(L.y, R.y) > 0)
            {
                var newR = Merge(L.Right, R);
                return new Tree<T>(L.x, L.y, L.Left, newR, _comparer);
            }
            else
            {
                var newL = Merge(L, R.Left);
                return new Tree<T>(R.x, R.y, newL, R.Right, _comparer);
            }
        }
    }
}
