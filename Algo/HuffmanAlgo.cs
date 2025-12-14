//https://habr.com/ru/companies/otus/articles/497566/ Алгоритм сжатия Хаффмана
// https://habr.com/ru/articles/144200/ Алгоритм Хаффмана на пальцах

using System.Text;

namespace Algo;

public static class HuffmanAlgo
{
    public static Stream Encode(ReadOnlySpan<char> input)
    {
        Dictionary<char, int> frequencies = HuffmanAlgo.BuildFrequnciesDictionary(input);

        Structures.PriorityQueue<HuffmanNode, int> queue = HuffmanAlgo.BuildQueue(frequencies);

        HuffmanNode root = HuffmanAlgo.BuildTree(queue);

        Dictionary<char, string> codes = HuffmanAlgo.BuildCodes(root, "");

        string encoded = HuffmanAlgo.Encode(input, codes);

        string headers = HuffmanAlgo.EncodeHeaders(codes);

        return HuffmanAlgo.WriteDataToStream(encoded, headers);
    }


    public static string Decode(Stream input)
    {
        StringBuilder stringBuilder = new ();
        using BinaryReader reader = new (input);
        int numBytesInHeader = reader.ReadInt32();
        byte[] headerBytes = reader.ReadBytes(numBytesInHeader);
        string headers = Encoding.UTF8.GetString(headerBytes);

        Dictionary<string, char> codes = HuffmanAlgo.DecodeHeaders(headers);

        int ch;
        string bitCode = "";
        while ((ch = reader.Read()) != -1) // Read() возвращает int, -1 — конец потока
        {
            char bit = (char) ch;
            bitCode += bit;

            if (codes.TryGetValue(bitCode, out char symbol))
            {
                stringBuilder.Append(symbol);
                bitCode = "";
            }
        }


        return stringBuilder.ToString();
    }

    private static Stream WriteDataToStream(string encoded, string headers)
    {
        Stream stream = new MemoryStream();
        using BinaryWriter writer = new (stream, Encoding.UTF8, true);

        int numBytesForEncoding = Encoding.UTF8.GetByteCount(headers);
        byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
        byte[] bytes = Encoding.UTF8.GetBytes(encoded);
        writer.Write(numBytesForEncoding);

        writer.Write(headerBytes);

        writer.Write(bytes);

        return stream;
    }

    private static string EncodeHeaders(Dictionary<char, string> codes)
    {
        StringBuilder sb = new ();
        foreach (KeyValuePair<char, string> kvc in codes)
        {
            sb.Append(kvc.Key);
            sb.Append(kvc.Value);
            sb.Append(';');
        }
        return sb.ToString();
    }

    private static string Encode(ReadOnlySpan<char> input, Dictionary<char, string> codes)
    {
        StringBuilder sb = new ();
        foreach (char c in input)
        {
            sb.Append(codes[c]);
        }

        return sb.ToString();
    }

    private static Dictionary<string, char> DecodeHeaders(string encoded)
    {
        Dictionary<string, char> result = new ();
        string[] pairs = encoded.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string pair in pairs)
        {
            if (pair.Length > 1)
            {
                char key = pair[0];
                string value = pair.Substring(1);
                result[value] = key;
            }
        }
        return result;
    }


    private static Dictionary<char, int> BuildFrequnciesDictionary(ReadOnlySpan<char> input)
    {

        Dictionary<char, int> frequencies = new ();
        foreach (char symbol in input)
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

        Structures.PriorityQueue<HuffmanNode, int> queue = new (default (IComparer<int>));
        foreach (KeyValuePair<char, int> kv in frequencies)
        {
            HuffmanNode n = new()
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

            HuffmanNode parent = new()
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
        Dictionary<char, string> codes = new ();
        Stack<(HuffmanNode, string)> stack = new ();

        stack.Push((node, code));

        while (stack.Count > 0)
        {
            (HuffmanNode, string) n = stack.Pop();
            node = n.Item1;
            code = n.Item2;
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