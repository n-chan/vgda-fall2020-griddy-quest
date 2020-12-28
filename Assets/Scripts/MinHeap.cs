using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinHeap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;
    //int g = 0;

    public MinHeap(int size) {
        items = new T[size];
    }

    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;

        //If last item, do nothing. If there's two items, swap. Otherwise, sort down.
        if (currentItemCount == 1) {
            //Do Nothing.
        }
        else if (currentItemCount == 2) {
            Swap(items[0], items[1]);
        }
        else {
            SortDown(items[0]);
        }
        
        return firstItem;
    }

    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    public void UpdateItem(T item) {
        SortUp(item);
    }

    public int Count {
        get {
            return currentItemCount;
        }
    }
    public void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;

        //This is saying that if item < parent
        while (item.CompareTo(items[parentIndex]) > 0) {
            Swap(item, items[parentIndex]);
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }
    
    
    public void SortDown(T item) {
        int childIndexLeft = item.HeapIndex * 2 + 1;
        int childIndexRight = item.HeapIndex * 2 + 2;
        int swapIndex = childIndexLeft;

        while (childIndexLeft < currentItemCount) {
            swapIndex = childIndexLeft;
            if (childIndexRight < currentItemCount && (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)) {
                swapIndex = childIndexRight;
            }
            Swap(item, items[swapIndex]);
            childIndexLeft = item.HeapIndex * 2 + 1;
            childIndexRight = item.HeapIndex * 2 + 2;
        }
    }
    
    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}