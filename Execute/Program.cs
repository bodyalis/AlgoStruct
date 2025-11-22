using System;
using System.Collections.Generic;
using System.Threading;

namespace Execute;

public static partial class  Program
{
    public static void Main()
    {
        // TestPriorityQeueue();
        TestHuffman();
    }

    public static void TestHuffman()
    {

        List<byte[]> list = new List<byte[]>();
        while (true)
        {
            byte[] b = new byte[1_000_000];
            Thread.Sleep(100);
            list.Add(b);
        }
        string s = "beep boop beer!";
        string huffman = HuffmanAlgo.Encode(s);
        
        Console.WriteLine(huffman);
    }
}
