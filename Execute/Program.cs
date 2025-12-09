using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HuffmanAlgorithm.Univer.Lab_1;

namespace Execute;

public static partial class Program
{
    public static void Main()
    {
        // TestPriorityQeueue();
        // Program.TestHuffman();

        // var t1 = new Task_1();
        // List<(Task_1.Item item, double fraction)> result = t1.Solve();
        // foreach (var tuple in result)
        // {
        //     Console.WriteLine($"Предмет {tuple.item.Id}: {tuple.fraction:P1} ({tuple.item.Weight * tuple.fraction:F0} веса)");
        // }

        var t2 = new Task_2();
        List<(Task_2.Coin coin, int count)> result2 = t2.Solve();
        foreach (var tuple2 in result2)
        {
            Console.WriteLine($"Монета {tuple2.coin.Value} - {tuple2.count}");
        }
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
