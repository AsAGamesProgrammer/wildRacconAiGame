using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PF_Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

	public PF_Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // Add to the heap.
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
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
        // Only ever going to sort upwards.
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }
    
    public bool Contains(T item)
    {
        // Check if two items are equal.
        return Equals(items[item.HeapIndex], item);
    }

    public void SortDown(T item)
    {
        while(true)
        {
            // Get child node index.
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;

            int swapIndex = 0;

            if(childIndexLeft < currentItemCount)
            {
                // Set left child as swap index by default.
                swapIndex = childIndexLeft;

                // Check if the right child should be used instead.
                if(childIndexRight < currentItemCount)
                {
                    // Check if the node using the right index has a lower fCost.
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Check if the swap index node has a lower fCost.
                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    // Swap the nodes.
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

    public void SortUp(T item)
    {
        // Cache the index of the parent node.
        int parentIndex = (item.HeapIndex - 1) / 2;

        while(true)
        {
            // Get the parent node using the index;
            T parentItem = items[parentIndex];

            // Priority based on low fCost.
            // Higher priority returns 1.
            // Equal priority returns 0.
            // Lower priority returns -1.
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

    public void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        // Temp value.
        int itemAIndex = itemA.HeapIndex;

        // Swap the values.
        itemA.HeapIndex = itemB.HeapIndex;

        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}