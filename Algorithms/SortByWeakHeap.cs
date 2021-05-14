using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public class SortByWeakHeap<T> : ISort<T>
    {
        private T buffer;
        private int heapSize;
        public T[] outputArr { get; set; }
        private T[] inputArr;
        private int[] bit;
        private IComparer<T> _comparer;
        public SortByWeakHeap(T[] array, IComparer<T> comparer)
        {
            heapSize = array.Length;
            outputArr = new T[heapSize];
            inputArr = new T[heapSize];
            Array.Copy(array, inputArr, heapSize);
            bit = new int[(heapSize - 1) / 2];
            _comparer = comparer;
        }
        public T[] Sort()
        {
            Array.Copy(inputArr, outputArr, heapSize);
            for (int i = heapSize - 1; i > 0; i--)
            {
                Heapify(i);
            }
            //сделали слабую кучу
            //теперь корни куч перемещаем в конец
            while (heapSize > 0)
            {
                buffer = outputArr[0];
                outputArr[0] = outputArr[heapSize - 1];
                outputArr[heapSize - 1] = buffer;
                heapSize--;
                int i = 1;
                //спускаемся вниз по левым потомком
                while ((i - 1) < bit.Length && 2 * i + bit[i - 1] < heapSize)
                {
                    i = 2 * i + bit[i - 1];
                }
                //нашли его индекс
                if (heapSize > 1)//если длина 1 , то элементы уже поменялись местами в строках 28-30
                {
                    while (i != 0)
                    {
                        SwapRoot(0, i);
                        if (i % 2 == 0)
                        {
                            i = i / 2;
                        }
                        else
                        {
                            i = (i - 1) / 2;
                        }
                    }
                }
            }
            return outputArr;

        }
        internal void Heapify(int index)
        {
            int firstIndex = index;
            bool isRightParentFound = false;
            while (!isRightParentFound)//ищем правого прародителя
            {
                if (index % 2 == 0)
                {
                    index = index / 2;
                }
                else
                {
                    index = (index - 1) / 2;
                    isRightParentFound = true;
                }
            }
            SwapRoot(index, firstIndex);
        }
        internal void SwapRoot(int i, int j)
        {
            if (_comparer.Compare(outputArr[i], outputArr[j]) < 0)
            {
                if ((j - 1) < bit.Length)
                {
                    bit[j - 1]++;
                    bit[j - 1] = bit[j - 1] % 2;
                }
                buffer = outputArr[i];
                outputArr[i] = outputArr[j];
                outputArr[j] = buffer;
            }
        }
    }
}
