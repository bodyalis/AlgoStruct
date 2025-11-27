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

    private Dictionary<int, LinkedList<Node>> _countNodeMap = new();
    private Dictionary<TKey, Node> _keyNodeMap = new();
    private int _capacity;

    public LfuCache(int capacity)
    {
        EnsureCapacityIsCorrect(ref capacity);
        _capacity = capacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacityIsCorrect(ref int capacity)
    {
        if (capacity <= 0)
            throw new InvalidOperationException("Capacity must be greater than or equal to zero.");
    }


    private void AddNodeToMap(Node node)
    {
        if (!_countNodeMap.TryGetValue(node.Count, out LinkedList<Node>? list))
        {
            list = new LinkedList<Node>();
            _countNodeMap[node.Count] = list;
        }

        list.AddFirst(node);

        _keyNodeMap[node.Key] = node;
    }


    public void Put(TKey key, TValue value)
    {
        if (_keyNodeMap.TryGetValue(key, out Node? node))
        {
            int prevIdx = node.Count;
            LinkedList<Node> nodes = _countNodeMap[prevIdx];
            nodes.Remove(node);
            if (nodes.Count == 0)
            {
                _countNodeMap.Remove(prevIdx);
            }

            node.Value = value;
            node.Count = 0;

            AddNodeToMap(node);
        }
        else
        {
            if (_keyNodeMap.Count == _capacity)
            {
                // Удаляем наименее редкий элемент
                int min = GetMinKey();
                LinkedList<Node> nodes = _countNodeMap[min];
                Node lastNode = nodes.Last();
                _keyNodeMap.Remove(lastNode.Key);
                nodes.RemoveLast();

                if (nodes.Count == 0)
                {
                    _countNodeMap.Remove(min);
                }
            }

            node = new Node();
            node.Value = value;
            node.Key = key;
            node.Count = 0;
            
            AddNodeToMap(node);
        }

        // Если элемент уже есть
        //   Сбрасываем счетчик обращений
        //   Обновляем значение
        //return;

        // Если коллекция полная
        //    Удаляем элемент с наименьшим кол-вом обращений

        // Добавляем элемент в коллекцию
        // Устанавливаем счетчик обращений в 1
    }

    private int GetMinKey()
    {
        Dictionary<int, LinkedList<Node>>.KeyCollection keys = _countNodeMap.Keys;
        int min = 0;
        foreach (int item in keys)
        {
            min = Math.Min(item, min);
        }

        return min;
    }

    public TValue Get(TKey key)
    {
        // если элемента не существует - ошибка
        // иначе - возвращаем значение и увеличиваем счетчик обращений

        if (!_keyNodeMap.TryGetValue(key, out Node? node))
        {
            throw new KeyNotFoundException(key.ToString());
        }

        _countNodeMap[node.Count].Remove(node);
        if (_countNodeMap[node.Count].Count == 0)
        {
            _countNodeMap.Remove(node.Count);
        }

        node.Count++;
        AddNodeToMap(node);

        return node.Value;
    }
}

public interface ILfuCache<TKey, TValue> where TKey : notnull
{
    public void Put(TKey key, TValue value);
    public TValue Get(TKey key);
}