using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public class SortByBottomUp<T> : ISort<T>
    {
        private T buffer;
        private int heapSize;
        public T[] outputArr { get; set; }
        private T[] inputArr;
        private IComparer<T> _comparer;
        public SortByBottomUp(T[] array, IComparer<T> comparer)
        {
            heapSize = array.Length;
            outputArr = new T[heapSize];
            inputArr = new T[heapSize];
            Array.Copy(array, inputArr, heapSize);
            _comparer = comparer;
        }
        public T[] Sort()
        {
            Array.Copy(inputArr, outputArr, heapSize);
            for (int i = (heapSize - 1) / 2; i >= 0; i--)
            {
                Heapify(i);
            }
            while (heapSize > 1)
            {
                buffer = outputArr[0];
                outputArr[0] = outputArr[heapSize - 1];
                outputArr[heapSize - 1] = buffer;
                heapSize--;
                if (heapSize > 1)
                {
                    Heapify(0);
                }
            }
            return outputArr;
        }
        private void Heapify(int indexOfLowerElement)
        {
            bool isItNotLastLevel = true;
            int firstindex = indexOfLowerElement;
            buffer = outputArr[indexOfLowerElement];
            while (isItNotLastLevel)
            {
                if ((2 * indexOfLowerElement + 1) < heapSize)//если у него есть дети
                {
                    if ((2 * indexOfLowerElement + 2) < heapSize)//2 ребенка
                    {
                        if (_comparer.Compare(outputArr[2 * indexOfLowerElement + 1], outputArr[indexOfLowerElement * 2 + 2]) > 0)
                        {
                            indexOfLowerElement = 2 * indexOfLowerElement + 1;
                        }
                        else
                        {
                            indexOfLowerElement = 2 * indexOfLowerElement + 2;
                        }
                    }
                    else//один ребенок
                    {
                        indexOfLowerElement = 2 * indexOfLowerElement + 1;
                        isItNotLastLevel = false;
                    }
                }
                else
                {
                    isItNotLastLevel = false;
                }
            }
            //дошли до низу, теперь надо подниматься и искать больший элемент.

            bool isLargestItemFound = true;
            while (isLargestItemFound && indexOfLowerElement >= firstindex)
            {
                if (_comparer.Compare(outputArr[indexOfLowerElement], buffer) > 0)
                {
                    buffer = outputArr[indexOfLowerElement];
                    outputArr[indexOfLowerElement] = outputArr[firstindex];
                    indexOfLowerElement = (indexOfLowerElement - 1) / 2;
                    while (indexOfLowerElement >= firstindex)
                    {
                        var temp = outputArr[indexOfLowerElement];
                        outputArr[indexOfLowerElement] = buffer;
                        buffer = temp;
                        if (indexOfLowerElement != 0)
                            indexOfLowerElement = (indexOfLowerElement - 1) / 2;
                        else
                            break;
                    }
                    isLargestItemFound = false;
                }
                else
                {
                    if (indexOfLowerElement != 0)
                        indexOfLowerElement = (indexOfLowerElement - 1) / 2;
                    else
                        break;
                }
            }
        }
    }
}
