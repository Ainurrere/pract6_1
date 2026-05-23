using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pract6_1
{
    class BinarySearchTree<T> where T : IComparable<T>
{
    public Node<T>? Root { get; private set; }

    public void Insert(T value)
    {
        Node<T> newNode = new Node<T>(value);

        if (Root == null)
        {
            Root = newNode;
            return;
        }

        Node<T> current = Root;

        while (true)
        {
            int cmp = value.CompareTo(current.Value);

            if (cmp < 0)
            {
                if (current.Left == null)
                {
                    current.Left = newNode;
                    return;
                }

                current = current.Left;
            }
            else if (cmp > 0)
            {
                if (current.Right == null)
                {
                    current.Right = newNode;
                    return;
                }

                current = current.Right;
            }
            else
            {
                return;
            }
        }
    }

    public bool Search(T value)
    {
        return SearchNode(value) != null;
    }

    public bool Search(T value, out int comparisons)
    {
        comparisons = 0;

        Node<T>? current = Root;

        while (current != null)
        {
            comparisons++;

            int cmp = value.CompareTo(current.Value);

            if (cmp == 0)
                return true;

            if (cmp < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        return false;
    }

    private Node<T>? SearchNode(T value)
    {
        Node<T>? current = Root;

        while (current != null)
        {
            int cmp = value.CompareTo(current.Value);

            if (cmp == 0)
                return current;

            if (cmp < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        return null;
    }

    public void Delete(T value)
    {
        Root = DeleteNode(Root, value);
    }

    private Node<T>? DeleteNode(Node<T>? node, T value)
    {
        if (node == null)
            return null;

        int cmp = value.CompareTo(node.Value);

        if (cmp < 0)
        {
            node.Left = DeleteNode(node.Left, value);
        }
        else if (cmp > 0)
        {
            node.Right = DeleteNode(node.Right, value);
        }
        else
        {
            if (node.Left == null)
                return node.Right;

            if (node.Right == null)
                return node.Left;

            Node<T> minNode = FindMinNode(node.Right);

            node.Value = minNode.Value;
            node.Right = DeleteNode(node.Right, minNode.Value);
        }

        return node;
    }

    public T Min()
    {
        if (Root == null)
            throw new InvalidOperationException("Дерево пустое.");

        return FindMinNode(Root).Value;
    }

    private Node<T> FindMinNode(Node<T> node)
    {
        Node<T> current = node;

        while (current.Left != null)
            current = current.Left;

        return current;
    }

    public T Max()
    {
        if (Root == null)
            throw new InvalidOperationException("Дерево пустое.");

        Node<T> current = Root;

        while (current.Right != null)
            current = current.Right;

        return current.Value;
    }

    // PreOrder итеративно
    public List<T> PreOrder(Node<T>? node)
    {
        List<T> result = new List<T>();

        if (node == null)
            return result;

        Stack<Node<T>> stack = new Stack<Node<T>>();
        stack.Push(node);

        while (stack.Count > 0)
        {
            Node<T> current = stack.Pop();

            result.Add(current.Value);  

            if (current.Right != null)
                stack.Push(current.Right);

            if (current.Left != null)
                stack.Push(current.Left);
        }

        return result;
    }

    // InOrder рекурсивно
    public List<T> InOrderTraversal(Node<T>? node)
    {
        List<T> result = new List<T>();

        InOrder(node, result);

        return result;
    }

    private void InOrder(Node<T>? node, List<T> result)
    {
        if (node == null)
            return;

        InOrder(node.Left, result);
        result.Add(node.Value);
        InOrder(node.Right, result);
    }

    // PostOrder рекурсивно
    public List<T> PostOrder(Node<T>? node)
    {
        List<T> result = new List<T>();

        PostOrderRec(node, result);

        return result;
    }

    private void PostOrderRec(Node<T>? node, List<T> result)
    {
        if (node == null)
            return;

        PostOrderRec(node.Left, result);
        PostOrderRec(node.Right, result);

        result.Add(node.Value);
    }

    public List<T> LevelOrder(Node<T>? node)
    {
        List<T> result = new List<T>();

        if (node == null)
            return result;

        Queue<Node<T>> queue = new Queue<Node<T>>();
        queue.Enqueue(node);

        while (queue.Count > 0)
        {
            Node<T> current = queue.Dequeue();

            result.Add(current.Value);

            if (current.Left != null)
                queue.Enqueue(current.Left);

            if (current.Right != null)
                queue.Enqueue(current.Right);
        }

        return result;
    }

    public void PrintTree()
{
    if (Root == null)
    {
        Console.WriteLine("Дерево пустое.");
        return;
    }

    Console.WriteLine("Root: " + Root.Value);

    PrintNode(Root.Left, "", true, "L");
    PrintNode(Root.Right, "", false, "R");
}

private void PrintNode(Node<T>? node, string indent, bool isLeft, string label)
{
    if (node == null)
        return;

    if (isLeft)
        Console.Write(indent + "├── ");
    else
        Console.Write(indent + "└── ");

    ConsoleColor oldColor = Console.ForegroundColor;

    if (label == "L")
        Console.ForegroundColor = ConsoleColor.Green;
    else
        Console.ForegroundColor = ConsoleColor.Cyan;

    Console.WriteLine(label + ": " + node.Value);

    Console.ForegroundColor = oldColor;

    string newIndent;

    if (isLeft)
        newIndent = indent + "│   ";
    else
        newIndent = indent + "    ";

    PrintNode(node.Left, newIndent, true, "L");
    PrintNode(node.Right, newIndent, false, "R");
}

    public int Height(T value)
    {
        Node<T>? node = SearchNode(value);

        if (node == null)
            return -1;

        return GetHeight(node);
    }

    private int GetHeight(Node<T>? node)
    {
        if (node == null)
            return -1;

        int leftHeight = GetHeight(node.Left);
        int rightHeight = GetHeight(node.Right);

        if (leftHeight > rightHeight)
            return leftHeight + 1;

        return rightHeight + 1;
    }

    public int Depth(T value)
    {
        Node<T>? current = Root;
        int depth = 0;

        while (current != null)
        {
            int cmp = value.CompareTo(current.Value);

            if (cmp == 0)
                return depth;

            if (cmp < 0)
                current = current.Left;
            else
                current = current.Right;

            depth++;
        }

        return -1;
    }

    public int BalanceFactor(Node<T>? node)
    {
        if (node == null)
            return 0;

        return GetHeight(node.Left) - GetHeight(node.Right);
    }

    public bool IsBalanced()
    {
        return IsBalancedNode(Root);
    }

    private bool IsBalancedNode(Node<T>? node)
    {
        if (node == null)
            return true;

        int bf = BalanceFactor(node);

        if (bf < -1 || bf > 1)
            return false;

        return IsBalancedNode(node.Left) && IsBalancedNode(node.Right);
    }
}
}