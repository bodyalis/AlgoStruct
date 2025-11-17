using System;
using System.Collections.Generic;

namespace Execute;

public static partial class  Program
{
    
static void TestPriorityQeueue()
{

    Structures.PriorityQueue<string, int> q = new (default (IComparer<int>));
    q.Enqueue("т", 6);
    q.Enqueue("и", 2);
    q.Enqueue("в", 4);
    q.Enqueue("е", 5);
    q.Enqueue("П", 0);
    q.Enqueue("р", 1);
    q.Enqueue("р", 1);
    q.Enqueue("р", 1);
    q.Enqueue("е", 5);
    q.Enqueue("и", 2);

    while (q.Count > 0)
    {
        Console.WriteLine(q.Dequeue());
    }
}

}
