using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils;

public class GameArrayList<T>
{
    private T [] Items; // => Asigns the space in memory
    private int ElementCount; // => Number of elements currently used
    private int Capacity;


    public GameArrayList(int capacity = 4)
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
            Items[i] = Items[i + 1];
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

    public T RemoveFirst()
    {
        if (head == null)
            throw new InvalidOperationException("Empty list.");

        T value = head.Value;
        head = head.Next;

        if (head != null)
            head.Prev = null;
        else
            tail = null;

        count--;
        return value;
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

// Circular linked list
// This structure will only be used by the server to maintain a turn based order

public class ClientNode
{
    public int Data { get; set; }

    public ClientNode? Next { get; set; }

    public ClientNode(int data)
    {
        Data = data;
        Next = null;
    }

}

public static class ClientList
{
    // Entry point node
    public static ClientNode? head;
    public static ClientNode? selected;

    public static void Add(int data)
    {
        ClientNode newNode = new ClientNode(data);

        if (head == null)
        {
            head = newNode;
            head.Next = head;
            selected = head;
        }

        else
        {
            ClientNode tempNode = head;
            while (tempNode!.Next != head)
                tempNode = tempNode.Next!;

            tempNode.Next = newNode;
            newNode.Next = head;
        }
    }

    public static int Traverse()
    {
        ClientNode currentSelection = selected!;
        selected = selected!.Next;
        return currentSelection.Data;
    }
}


public class GameQueue<T>
{
    private GameLinkedList<T> list = new GameLinkedList<T>();

    // Adding to end
    public void Enqueue(T item)
    {
        list.AddBack(item);
    }

    // Removing first
    public T Dequeue()
    {
        if (list.Count == 0)
            throw new InvalidOperationException("Queue is empty");
        return list.RemoveFirst();
    }

}