using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public class TournamentSort<T> : ISort<T>
    {
        private int length;
        public T[] mainArray;
        protected T[] losers;
        private T[] inputArr;
        public T[] outputArr { get; set; }
        public int indexOfMinimalOrFreePlace;
        public int indOfCurrentElement;
        public bool isTournamentNeeded; //с помощью него смотрим, нужно ли снова проводить турнир
        public DataNode<T>[] Tree;
        private IComparer<T> _comparer;
        public int heapSize = 63;
        public TournamentSort(T[] array, IComparer<T> comparer)
        {
            length = array.Length;
            if (length < 1000)
                heapSize = 7;
            else if (length < 10000)
                heapSize = 511;
            else
                heapSize = 8191;
            mainArray = new T[length];
            inputArr = new T[length];
            Array.Copy(array, inputArr, length);
            Tree = new DataNode<T>[heapSize];
            losers = new T[length];
            outputArr = new T[length];// массив победителей
            isTournamentNeeded = true;
            _comparer = comparer;
        }
        public T[] Sort()
        {
            Array.Copy(inputArr, mainArray, length);
            while (isTournamentNeeded)
            {
                BuildTree();
                int countOfWinners = 0;
                int countOfLosers = 0;//куча заполнена
                //теперь берем каждый элемент массива и смотрим, больше ли он вершины(или равен)
                //иначе отправляем в проигравших
                for (int i = indOfCurrentElement; i < length; i++)
                {
                    if (_comparer.Compare(mainArray[i], Tree[0].data) > 0 || _comparer.Compare(mainArray[i], Tree[0].data) == 0)
                    {
                        outputArr[countOfWinners] = Tree[0].data;
                        climbTheMimimum(0);
                        Tree[indexOfMinimalOrFreePlace] = new DataNode<T>(mainArray[i]);
                        upNewItem(indexOfMinimalOrFreePlace);
                        countOfWinners++;
                    }
                    else
                    {
                        losers[countOfLosers] = mainArray[i];
                        countOfLosers++;
                    }
                }
                for (int i = 0; i < heapSize; i++)
                {
                    outputArr[countOfWinners] = Tree[0].data;
                    climbTheMimimum(0);
                    countOfWinners++;
                }
                if (countOfLosers > 0)
                {
                    losers.CopyTo(mainArray, 0);
                    for (int i = countOfLosers; i < length; i++)
                    {
                        mainArray[i] = outputArr[i - countOfLosers];
                    }
                }
                else
                    isTournamentNeeded = false;
            }
            return outputArr;
        }
        private void BuildTree()
        {
            Tree[0] = default;//делаю, чтобы когда в куче один элемент, ме его берем и куча опустеет.

            for (int i = (heapSize - 1) / 2; i < heapSize; i++)//заполняем нижние узля дерева
            {
                Tree[i] = new DataNode<T>(mainArray[i - (heapSize - 1) / 2]);
            }
            indOfCurrentElement = (heapSize - 1) / 2 + 1;
            //сначала строим дерево, в вершине которого стоит минимальный элемент. "проигравшие" и "выйгрывшие" пусты.
            for (int i = (heapSize - 1) / 2 - 1; i >= 0; i--)
            {
                climbTheMimimum(i);
                if (indOfCurrentElement < length)
                {
                    Tree[indexOfMinimalOrFreePlace] = new DataNode<T>(mainArray[indOfCurrentElement]);
                    indOfCurrentElement++;
                    upNewItem(indexOfMinimalOrFreePlace);
                }

            }
        }
        private void climbTheMimimum(int index)
        {
            bool isItNotLastLevel = true;
            while (index >= 0 && (index * 2 + 1) < heapSize && (index * 2 + 2) < heapSize)
            {
                if (Tree[index * 2 + 1] != null && Tree[index * 2 + 2] != null)
                {
                    if (_comparer.Compare(Tree[index * 2 + 1].data, Tree[index * 2 + 2].data) > 0)
                    {
                        indexOfMinimalOrFreePlace = index * 2 + 2;
                    }
                    else
                    {
                        indexOfMinimalOrFreePlace = index * 2 + 1;
                    }
                }
                else if (Tree[index * 2 + 1] == null && Tree[index * 2 + 2] != null)
                {
                    indexOfMinimalOrFreePlace = 2 * index + 2;
                }
                else if (Tree[index * 2 + 1] != null && Tree[index * 2 + 2] == null)
                {
                    indexOfMinimalOrFreePlace = 2 * index + 1;
                }
                else
                {
                    isItNotLastLevel = false;
                }
                if (isItNotLastLevel)
                {
                    Tree[index] = Tree[indexOfMinimalOrFreePlace];
                    Tree[indexOfMinimalOrFreePlace] = default;
                    index = indexOfMinimalOrFreePlace;
                }
                else
                    break;
            }
        }
        private void upNewItem(int index)
        {
            int parentInd = (index - 1) / 2;
            while (parentInd >= 0 && Tree[parentInd] != null && (_comparer.Compare(Tree[parentInd].data, Tree[index].data) > 0))
            {
                DataNode<T> temp = new DataNode<T>(Tree[index].data);
                Tree[index] = Tree[parentInd];
                Tree[parentInd] = temp;
                index = parentInd;
                parentInd = (parentInd - 1) / 2;
            }
        }
    }
    public class DataNode<T>
    {
        public T data;
        public DataNode(T data)
        {
            this.data = data;
        }
    }
}
