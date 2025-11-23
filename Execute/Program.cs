using System;
using System.Collections.Generic;
using System.IO;
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
        
        string s = "beep boop beer!";
        Stream huffman = HuffmanAlgo.Encode(s);

        huffman.Position = 0;
        
        
        Console.WriteLine(huffman);

        string s2 = HuffmanAlgo.Decode(huffman);
        Console.WriteLine(s2);
    }
}
