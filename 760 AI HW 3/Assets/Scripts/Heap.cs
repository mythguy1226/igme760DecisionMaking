using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    // Private fields for holding items
    // on the heap and for tracking total count
    T[] items;
    int currentItemCount;

    // Constructor
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // Method used for adding new items to the heap
    public void Add(T item)
    {
        // Add item to end of the heap
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;

        // Sort from the bottom up
        // and increment total item count
        SortUp(item);
        currentItemCount++;
    }

    // Method for removing first item on the heap
    // and then sorting downwards
    public T RemoveFirst()
    {
        // Get the first item and reduce heap count
        T firstItem = items[0];
        currentItemCount--;

        // Take the item from the bottom of the heap
        // and bring it to the top
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;

        // Sort the heap from top down
        SortDown(items[0]);

        // Return the item being removed
        return firstItem;
    }

    // Method for updating items on the heap
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    // Property for returning all
    // items on the heap
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    // Method used to check if item exists
    // on the heap
    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    // Method used for sorting downwards on the heap
    void SortDown(T item)
    {
        // Iterate through heap until you find a node
        // that doesn't have to be swapped with the item
        while (true)
        {
            // Get the left and right nodes branched
            // going downward on the heap
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRight = item.heapIndex * 2 + 2;
            int swapIndex = 0;

            // Check that the left index is less than the item count
            if(childIndexLeft < currentItemCount)
            {
                // Update swap index to left index
                swapIndex = childIndexLeft;

                // Check that the right index is less than item count
                if(childIndexRight < currentItemCount)
                {
                    // Update to the right index if needed
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Swap the item with the swap index
                // if the item is bigger
                if(item.CompareTo(items[swapIndex]) < 0)
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

    // Method used for sorting upwards on the heap
    void SortUp(T item)
    {
        // Get parent index
        int parentIndex = (item.heapIndex - 1) / 2;

        // Iterate through heap until you find a node
        // that doesn't have to be swapped with the item
        while(true)
        {
            // Get parent item from index
            T parentItem = items[parentIndex];

            // If the item is bigger than its parent
            // swap the two
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.heapIndex - 1) / 2;
        }
    }

    // Method used for swapping two items on the heap
    void Swap(T itemA, T itemB)
    {
        // Swap the two items
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        
        // Temp variable for storing proper index
        // then swap indices
        int itemAIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{ 
    // All classes using this interface need to
    // implement a property for getting and
    // setting the heap index
    int heapIndex
    {
        get;
        set;
    }
}
