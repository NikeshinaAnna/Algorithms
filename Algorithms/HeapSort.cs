using System;
using System.Collections.Generic;

namespace Algorithms
{
    public class HeapSort<T> : ISort<T>
    {
        private T buffer;
        private int heapSize;
        public T[] outputArr { get; set; }
        private IComparer<T> _comparer;
        private T[] inputArr;
        public HeapSort(T[] arr_param, IComparer<T> comparer)
        {
            heapSize = arr_param.Length;
            inputArr = new T[heapSize];
            Array.Copy(arr_param, inputArr, heapSize);
            outputArr = new T[heapSize];

            _comparer = comparer;
        }
        public T[] Sort()
        {
            Array.Copy(inputArr, outputArr, heapSize);
            //строим сортирующее дерево
            for (int i = (heapSize - 1) / 2; i >= 0; i--)
            {
                Heapify(i);
            }
            //дерево построено, теперь крупнейший элемент меняем местами с минимальным и снова просейка.
            while (heapSize > 0)
            {
                buffer = outputArr[0];
                outputArr[0] = outputArr[heapSize - 1];
                outputArr[heapSize - 1] = buffer;
                heapSize--;
                Heapify(0);
            }
            return outputArr;
        }
        internal void Heapify(int index)
        {
            int indOfHuge;
            bool isNotLastLevelAndNotLargest = true;
            while (isNotLastLevelAndNotLargest)
            {
                buffer = outputArr[index];
                if ((index * 2 + 1) < heapSize && (index * 2 + 2) < heapSize) //у элемента два ребенка
                {
                    if (_comparer.Compare(outputArr[index * 2 + 1], outputArr[index * 2 + 2]) > 0)
                        indOfHuge = index * 2 + 1;
                    else
                        indOfHuge = index * 2 + 2;
                    if (_comparer.Compare(outputArr[indOfHuge], buffer) > 0)
                    {
                        outputArr[index] = outputArr[indOfHuge];
                        outputArr[indOfHuge] = buffer;
                        index = indOfHuge;
                    }
                    else
                        isNotLastLevelAndNotLargest = false;
                }
                else if ((index * 2 + 1) < heapSize)// у эл-та один ребенок
                {
                    if (_comparer.Compare(outputArr[index * 2 + 1], buffer) > 0)
                    {
                        outputArr[index] = outputArr[index * 2 + 1];
                        outputArr[index * 2 + 1] = buffer;
                        index = index * 2 + 1;
                    }
                    else
                        isNotLastLevelAndNotLargest = false;
                }
                else//нет детей
                    isNotLastLevelAndNotLargest = false;
            }

        }
    }
}
