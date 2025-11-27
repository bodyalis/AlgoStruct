using System.Runtime.CompilerServices;

namespace Structures;

public class LfuCache<TKey, TValue> : ILfuCache<TKey, TValue> where TKey : notnull
{
    public class Node
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public int Count { get; set; }
    }

    Dictionary<int, LinkedList<Node>> _countNodeMap = new();
    Dictionary<TKey, Node> _keyNodeMap = new();
    
    public LfuCache(int capacity)
    {
        EnsureCapacityIsCorrect(ref capacity);
        
        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacityIsCorrect(ref int capacity)
    {
        if (capacity <= 0)
            throw new InvalidOperationException("Capacity must be greater than or equal to zero.");
    }
    

    

    public void Put(TKey key, TValue value)
    {
        if (_keyNodeMap.TryGetValue(key, out var node))
        {
            int prevIdx = node.Count;
            LinkedList<Node> list = _countNodeMap[prevIdx];
            list.Remove(node);
            if (list.Count == 0)
            {
                _countNodeMap.Remove(prevIdx);
            }
            
            
            node.Count++;
            
            
        }
        
        // Если элемент уже есть
        //   Увеличиваем счетчик обращений
        //   Обновляем значение
        //return;
        
        // Если коллекция полная
        //    Удаляем элемент с наименьшим кол-вом обращений
        
        // Добавляем элемент в коллекцию
        // Устанавливаем счетчик обращений в 1
    }

    public TValue Get(TKey key)
    {
        // если элемента не существует - ошибка
        // иначе - возвращаем значение и увеличиваем счетчик обращений
    }
}

public interface ILfuCache<TKey, TValue> where TKey : notnull
{
    public void Put(TKey key, TValue value);
    public  TValue Get(TKey key);
}