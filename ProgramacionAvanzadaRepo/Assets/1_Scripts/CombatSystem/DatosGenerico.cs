using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPile<T>
{
    public class Node
    {
        public T data;
        public Node next;

        public Node(T t)
        {
            next = null;
            data = t;
        }
    }

    private Node head;
    public GenericPile()
    {
        //Cuando se comienza una pila el primer elemento es nulo
        head = null;
    }

    public void AddItem(T newItem)
    {
        Node n = new Node(newItem);
        n.next = head;
        head = n;
    }

    public T GetHead()
    {
        if (head == null)
        {
            return default;
        }

        T data = head.data;
        head = head.next;
        return data;
    }
}
