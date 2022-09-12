using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinHeap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;
    public MinHeap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex],item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int leftIndex = 2 * item.HeapIndex + 1;
            int rightIndex = 2 * item.HeapIndex + 2;
            int swapIndex = 0;
            if (leftIndex < currentItemCount)
            {
                swapIndex = leftIndex;
                if (rightIndex < currentItemCount)
                {
                    if (items[leftIndex].CompareTo(items[rightIndex]) < 0)
                    {
                        swapIndex = rightIndex;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }


    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get; set; 
    }
}
