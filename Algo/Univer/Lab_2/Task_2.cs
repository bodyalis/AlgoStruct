namespace Algo.Univer.Lab_2;

public class Task_2
{
    public static void ShowIntBinary(int x)
    {
        for (int i = 31; i >= 0; i--)
        {
            bool bit = (x & (1 << i)) != 0;
            Console.Write(bit ? '1' : '0');
            if (i % 8 == 0)
            {
                Console.Write(' '); // group by bytes 
            }
        }

        Console.WriteLine();
    }

    public static void ShowIntHex(int number)
    {
        string result = "0x";

        for (int i = 7; i >= 0; i -= 1)
        {
            int part = (number >> (i * 4)) & 0xF;
            char digit = part < 10 ? (char)('0' + part) : (char)('A' + (part - 10));
            result += digit;
        }

        Console.WriteLine(result);
    }
}
