//https://habr.com/ru/companies/otus/articles/497566/ Алгоритм сжатия Хаффмана
// https://habr.com/ru/articles/144200/ Алгоритм Хаффмана на пальцах


using System.Text;

public static class HuffmanAlgo
{
    public static string Encode(ReadOnlySpan<char> input)
    {
        Dictionary<char, int> frequencies = HuffmanAlgo.BuildFrequnciesDictionary(input);

        Structures.PriorityQueue<HuffmanNode, int> queue = HuffmanAlgo.BuildQueue(frequencies);
        
        HuffmanNode root = HuffmanAlgo.BuildTree(queue);
        
        Dictionary<char, string> codes = HuffmanAlgo.BuildCodes(root, "");

        return Encode(input, codes);
    }

    private static string Encode(ReadOnlySpan<char> input, Dictionary<char, string> codes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in input)
        {
            sb.Append(codes[c]);
        }
        
        
        return sb.ToString();
    }

    // public static string Decode(Stream input)
    // {
    //     input = input ?? throw new ArgumentNullException(nameof(input));
    //
    //     if (input == Stream.Null || input.Length == 0)
    //     {
    //         return "";
    //     }
    //     
    //     
    // }
    
    private static Dictionary<char, int> BuildFrequnciesDictionary(ReadOnlySpan<char> input)
    {

        Dictionary<char, int> frequencies = new Dictionary<char, int>();
        foreach (var symbol in input)
        {
            if (!frequencies.ContainsKey(symbol))
            {
                frequencies[symbol] = 0;
            }

            frequencies[symbol]++;
        }
        return frequencies;
    }

    private static Structures.PriorityQueue<HuffmanNode, int> BuildQueue(Dictionary<char, int> frequencies)
    {

        Structures.PriorityQueue<HuffmanNode, int> queue = new (default);
        foreach (var kv in frequencies)
        {
            HuffmanNode n = new HuffmanNode()
            {
                Symbol = kv.Key,
                Frequency = kv.Value
            };
            queue.Enqueue(n, n.Frequency);
        }
        return queue;
    }

    private static HuffmanNode BuildTree(Structures.PriorityQueue<HuffmanNode, int> queue)
    {

        while (queue.Count > 1)
        {
            HuffmanNode left = queue.Dequeue();
            HuffmanNode right = queue.Dequeue();

            HuffmanNode parent = new HuffmanNode()
            {
                Symbol = null,
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };
            queue.Enqueue(parent, parent.Frequency);
        }
        HuffmanNode root = queue.Dequeue();
        return root;
    }

    private static Dictionary<char, string> BuildCodes(HuffmanNode node, string code)
    {
        Dictionary<char, string> codes = new Dictionary<char, string>();
        Stack<(HuffmanNode, string)> stack = new Stack<(HuffmanNode, string)>();

        stack.Push((node, code));
        
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            node = n.Item1;
            code =  n.Item2;
            if (node.Left == null && node.Right == null && node.Symbol != null)
            {
                codes[node.Symbol.Value] = code;
                continue;
            }

            if (node.Left != null)
            {
                stack.Push((node.Left, code + "0"));
            }

            if (node.Right != null)
            {
                stack.Push((node.Right, code + "1"));
            }
        }
        
        return codes;
    }
    
    
}

public class HuffmanNode
{
    public int Frequency { get; set; }
    public char? Symbol { get; set; }
    public HuffmanNode? Left { get; set; }
    public HuffmanNode? Right { get; set; }
}
