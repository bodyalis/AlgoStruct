namespace Algo.Univer.Lab_1;

public class HuffmanNode
{
    public int Frequency { get; set; }
    public char? Symbol { get; set; }
    public HuffmanNode? Left { get; set; }
    public HuffmanNode? Right { get; set; }
}

public class Task_5
{
    public double Solve()
    {
        var frequencies = new Dictionary<char, int>
        {
            { 'a', 5 }, 
            { 'b', 2 }, 
            { 'r', 2 },
            { 'c', 1 },
            { 'd', 1 }
        };

        var codes = BuildHuffmanCodes(frequencies);

         Console.WriteLine("\nКоды Хаффмана:");
         foreach (var kvp in codes.OrderBy(k => k.Key))
         {
             Console.WriteLine($"{kvp.Key}: {kvp.Value}");
         }
        
        double avgLength = CalculateAverageLength(frequencies, codes);
         Console.WriteLine($"\nСредняя длина кода: {avgLength:F4} бит");

        return avgLength;
    }

    static Dictionary<char, string> BuildHuffmanCodes(Dictionary<char, int> frequencies)
    {
        var pq = new PriorityQueue<HuffmanNode, int>();

        foreach (var kvp in frequencies)
        {
            pq.Enqueue(new HuffmanNode() { Frequency = kvp.Value, Symbol = kvp.Key }, kvp.Value);
        }
        
        while (pq.Count > 1)
        {
            HuffmanNode left = pq.Dequeue();
            HuffmanNode right = pq.Dequeue();

            HuffmanNode parent = new()
            {
                Symbol = null,
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };
            pq.Enqueue(parent, parent.Frequency);
        }

        var root = pq.Dequeue();
        var codes = new Dictionary<char, string>();
        GenerateCodes(root, "", codes);
        return codes;
    }

    static void GenerateCodes(HuffmanNode node, string code, Dictionary<char, string> codes)
    {
        if (node.Symbol.HasValue)
        {
            codes[node.Symbol.Value] = code;
            return;
        }

        if (node.Left != null)
        {
            GenerateCodes(node.Left, code + "0", codes);
        }

        if (node.Right != null)
        {
            GenerateCodes(node.Right, code + "1", codes);
        }
    }

    static double CalculateAverageLength(Dictionary<char, int> frequencies, Dictionary<char, string> codes)
    {
        int totalFreq = frequencies.Values.Sum();
        double totalLength = frequencies.Sum(kvp => kvp.Value * codes[kvp.Key].Length);
        return totalLength / totalFreq;
    }
}

// Коды Хаффмана:
// a: 0
// b: 110
// c: 1110
// d: 1111
// r: 10
//
// Средняя длина кода: 2,0909 бит
