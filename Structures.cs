using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils;

public class ArrayList<T>
{
    private T [] Items; // => Asigns the space in memory
    private int ElementCount; // => Number of elements currently used
    private int Capacity;


    public ArrayList(int capacity = 4)
    {
        Items = new T [capacity];
        ElementCount = 0;
        UpdateCapacity();
    }

    private void UpdateCapacity()
    {
        Capacity = Items.Length;
    }

    // Debugging public methods
    public int Count => ElementCount;
    public int AvailableSpace => Items.Length - ElementCount;
    public int GetCapacity => Capacity;



    public void Add( T value)
    {
        if (ElementCount == Capacity)
        {
            Resize();
        }
        Items[ElementCount++] = value;
    }

    private void Resize()
    {
        int newCapacity = Capacity * 2;
        T[] newArray = new T[newCapacity];

        for (int index = 0; index < Capacity; index++)
        {
            newArray[index] = Items[index];
        }

        Items = newArray;
        UpdateCapacity();
    }

    // Indexer "Helps the class object behave like an array"
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= ElementCount)
                throw new IndexOutOfRangeException();
            return Items[index];
        }

        set
        {
            if (index < 0 || index >= ElementCount)
                throw new IndexOutOfRangeException();
            Items[index] = value;
        }
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= ElementCount)
            return false;

        for (int i = index; i < ElementCount - 1; i++)
        {
            Items[i] = Items[i++];
        }

        ElementCount--;
        Items[ElementCount] = default!; // Clear last slot
        return true;
    }

}



public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }
    public Node<T>? Prev { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
        Prev = null;
    }

}


public class GameLinkedList<T> : IEnumerable<T>
{
    private Node<T>? head;
    private Node<T>? tail;
    private int count;


    public int Count => count;
    public bool IsEmpty => count == 0;


    public GameLinkedList()
    {
        head = null;
        tail = null;
        count = 0;
    }

    public void Addfront(T value)
    {
        Node<T> newNode = new Node<T>(value);

        if (head == null)
        {
            head = tail = newNode;
        }

        else
        {
            newNode.Next = head;
            head.Prev = newNode;
            head = newNode;
        }

        count++;
    }

    public void AddBack(T value)
    {
        Node<T> newNode = new Node<T>(value);

        if (tail == null)
        {
            head = tail = newNode;
        }

        else
        {
            tail.Next = newNode;
            newNode.Prev = tail;
            tail = newNode;
        }

        count++;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = head;

        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}