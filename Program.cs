using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pract6_1
{
    class Program
    {
        static void Main(string[] args)
        {
           BinarySearchTree<int> tree = new BinarySearchTree<int>();

        bool run = true;

        while (run)
        {
            Console.WriteLine();
            Console.WriteLine("===== Двоичное дерево поиска =====");
            Console.WriteLine("1. Добавить элемент");
            Console.WriteLine("2. Удалить элемент");
            Console.WriteLine("3. Найти элемент");
            Console.WriteLine("4. Min / Max");
            Console.WriteLine("5. Обходы дерева");
            Console.WriteLine("6. Характеристики узла");
            Console.WriteLine("7. Вывести дерево");
            Console.WriteLine("8. Тест двух деревьев");
            Console.WriteLine("9. Тест поиска на 10000 элементах");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddElement(tree);
                    break;

                case "2":
                    DeleteElement(tree);
                    break;

                case "3":
                    SearchElement(tree);
                    break;

                case "4":
                    ShowMinMax(tree);
                    break;

                case "5":
                    ShowTraversals(tree);
                    break;

                case "6":
                    ShowInfo(tree);
                    break;

                case "7":
                    tree.PrintTree();
                    break;

                case "8":
                    TestTwoTrees();
                    break;

                case "9":
                    TestSearchTime();
                    break;

                case "0":
                    run = false;
                    break;

                default:
                    Console.WriteLine("Нет такого пункта.");
                    break;
            }
        }
    }

    static int ReadInt(string text)
    {
        while (true)
        {
            Console.Write(text);

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Ошибка: нужно ввести целое число.");
        }
    }

    static void AddElement(BinarySearchTree<int> tree)
    {
        int value = ReadInt("Введите число: ");

        tree.Insert(value);

        Console.WriteLine("Элемент добавлен.");
        tree.PrintTree();
    }

    static void DeleteElement(BinarySearchTree<int> tree)
    {
        int value = ReadInt("Введите число для удаления: ");

        tree.Delete(value);

        Console.WriteLine("Удаление выполнено.");
        tree.PrintTree();
    }

    static void SearchElement(BinarySearchTree<int> tree)
    {
        int value = ReadInt("Введите число для поиска: ");

        bool found = tree.Search(value, out int comparisons);

        if (found)
            Console.WriteLine("Элемент найден.");
        else
            Console.WriteLine("Элемент не найден.");

        Console.WriteLine("Количество сравнений: " + comparisons);

        tree.PrintTree();
    }

    static void ShowMinMax(BinarySearchTree<int> tree)
    {
        try
        {
            Console.WriteLine("Минимум: " + tree.Min());
            Console.WriteLine("Максимум: " + tree.Max());
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void ShowTraversals(BinarySearchTree<int> tree)
    {
        Console.WriteLine("PreOrder:   " + ListToString(tree.PreOrder(tree.Root)));
        Console.WriteLine("InOrder:    " + ListToString(tree.InOrderTraversal(tree.Root)));
        Console.WriteLine("PostOrder:  " + ListToString(tree.PostOrder(tree.Root)));
        Console.WriteLine("LevelOrder: " + ListToString(tree.LevelOrder(tree.Root)));
    }

    static string ListToString(List<int> list)
    {
        string result = "";

        for (int i = 0; i < list.Count; i++)
        {
            result += list[i] + " ";
        }

        return result;
    }

    static void ShowInfo(BinarySearchTree<int> tree)
    {
        int value = ReadInt("Введите значение узла: ");

        int height = tree.Height(value);
        int depth = tree.Depth(value);

        if (height == -1 || depth == -1)
        {
            Console.WriteLine("Такого узла нет.");
            return;
        }

        Console.WriteLine("Высота узла: " + height);
        Console.WriteLine("Глубина узла: " + depth);
        Console.WriteLine("Баланс корня: " + tree.BalanceFactor(tree.Root));
        Console.WriteLine("Дерево сбалансировано: " + tree.IsBalanced());
    }

    static void TestTwoTrees()
    {
        int[] a = { 10, 20, 30, 40, 50, 60, 70 };
        int[] b = { 40, 20, 10, 30, 60, 50, 70 };

        BinarySearchTree<int> treeA = new BinarySearchTree<int>();
        BinarySearchTree<int> treeB = new BinarySearchTree<int>();

        for (int i = 0; i < a.Length; i++)
            treeA.Insert(a[i]);

        for (int i = 0; i < b.Length; i++)
            treeB.Insert(b[i]);

        Console.WriteLine();
        Console.WriteLine("Дерево A:");
        treeA.PrintTree();

        Console.WriteLine();
        Console.WriteLine("Дерево B:");
        treeB.PrintTree();

        treeA.Search(70, out int cmpA);
        treeB.Search(70, out int cmpB);

        Console.WriteLine();
        Console.WriteLine("Поиск 70 в дереве A: " + cmpA + " сравнений");
        Console.WriteLine("Поиск 70 в дереве B: " + cmpB + " сравнений");

        Console.WriteLine();
        Console.WriteLine("Ответ:");
        Console.WriteLine("В дереве A нужно 7 сравнений, потому что оно стало цепочкой вправо.");
        Console.WriteLine("В дереве B нужно 3 сравнения: 40 -> 60 -> 70.");
        Console.WriteLine("Если вставлять элементы по порядку, обычное BST может стать цепочкой.");
        Console.WriteLine("В худшем случае поиск работает за O(n).");
        Console.WriteLine("В сбалансированном случае поиск работает за O(log n).");
    }

    static void TestSearchTime()
    {
        const int N = 10000;

        BinarySearchTree<int> sortedTree = new BinarySearchTree<int>();
        BinarySearchTree<int> randomTree = new BinarySearchTree<int>();

        for (int i = 1; i <= N; i++)
            sortedTree.Insert(i);

        int[] numbers = new int[N];

        for (int i = 0; i < N; i++)
            numbers[i] = i + 1;

        Random rnd = new Random();

        for (int i = 0; i < N; i++)
        {
            int j = rnd.Next(i, N);

            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        for (int i = 0; i < N; i++)
            randomTree.Insert(numbers[i]);

        int[] targets =
        {
            9991, 9992, 9993, 9994, 9995,
            9996, 9997, 9998, 9999, 10000
        };

        double sortedTime = AverageSearchTime(sortedTree, targets);
        double randomTime = AverageSearchTime(randomTree, targets);

        Console.WriteLine();
        Console.WriteLine("Среднее время поиска по 10 тестам:");
        Console.WriteLine("Дерево из отсортированных чисел: " + sortedTime + " мкс");
        Console.WriteLine("Дерево из случайных чисел:       " + randomTime + " мкс");
    }

    static double AverageSearchTime(BinarySearchTree<int> tree, int[] targets)
    {
        long totalTicks = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            Stopwatch sw = Stopwatch.StartNew();

            tree.Search(targets[i]);

            sw.Stop();

            totalTicks += sw.ElapsedTicks;
        }

        double avgTicks = totalTicks / targets.Length;
        double microseconds = avgTicks * 1000000.0 / Stopwatch.Frequency;

        return microseconds;
        }
    }
}