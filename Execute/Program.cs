using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Algo;
using HuffmanAlgorithm;

namespace Execute;

public static partial class Program
{
    public static void Main()
    {
        // TestPriorityQeueue();
        // Program.TestHuffman();
        
        // var unionFind = new UnionFind(5);
        // unionFind.Union(1, 2);
        // unionFind.Union(3, 4);
        // unionFind.Union(2, 3);
        object o = (int)ProcessingWay.None;
        Console.WriteLine(o.ToString());
    }

    public static void TestHuffman()
    {

        string s = "beep boop beer!";
        Stream huffman = HuffmanAlgo.Encode(s);

        huffman.Position = 0;


        Console.WriteLine(huffman);

        string s2 = HuffmanAlgo.Decode(huffman);
        Console.WriteLine(s2);

        Debug.Assert(s == s2);
    }
}

public enum ProcessingWay
{
    None = 0,
    Forward = 1,
    Backward = 2,
}
