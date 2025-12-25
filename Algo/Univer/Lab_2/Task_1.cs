using System.Runtime.InteropServices;

namespace Algo.Univer.Lab_2;

public static class Task_1
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IntBytes
    {
        [FieldOffset(0)] public int Value;
        [FieldOffset(0)] public byte B0;
        [FieldOffset(1)] public byte B1;
        [FieldOffset(2)] public byte B2;
        [FieldOffset(3)] public byte B3;
    }

    public static void ShowInt(int value)
    {
        Console.WriteLine(value);
    }

    public static void ShowIntBytes(IntBytes value)
    {
        Console.WriteLine($"{value.B0:X2} {value.B1:X2} {value.B2:X2} {value.B3:X2}");
    }
    public static void ShowIntBytes(int intVal)
    {
        IntBytes value = new()
        {
            Value = intVal
        };
        
        Console.WriteLine($"{value.B0:X2} {value.B1:X2} {value.B2:X2} {value.B3:X2}");
    }
}