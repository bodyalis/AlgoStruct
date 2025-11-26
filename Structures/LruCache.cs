using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Structures;

/// <summary>
/// Simple Lru Cache Implementation
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class LruCache<TKey, TValue> where TKey : notnull
{
    private class LruNode
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public LruNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateValue(TValue newValue)
        {
            Value = newValue;
        }
    }

    private readonly LinkedList<LruNode> _list = new ();
    private readonly Dictionary<TKey, LruNode> _dictionary;
    private int _capacity;

    public int Capacity => _capacity;
    public int Count => _list.Count;

    public LruCache(int capacity)
    {
        EnsureCorrectCapacity(capacity);

        _capacity = capacity;
        _dictionary = new Dictionary<TKey, LruNode>(capacity);
    }

    public TValue? GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (_dictionary.TryGetValue(key, out LruNode? node))
        {
            UpdateNodePosition(node);
            return node.Value;
        }

        TValue newValue = valueFactory(key);
        AddInternal(key, newValue);
        return newValue;
    }

    /// <summary>
    /// Update the capacity of the cache.
    /// </summary>
    /// <param name="newCapacity"></param>
    public void UpdateCapacity(int newCapacity)
    {
        EnsureCorrectCapacity(newCapacity);

        if (newCapacity < _capacity)
        {
            while (_list.Count > newCapacity)
            {
                LruNode lastNode = _list.Last!.Value;
                _dictionary.Remove(lastNode.Key);
                _list.RemoveLast();
            }
        }

        _capacity = newCapacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCorrectCapacity(int newCapacity)
    {
        if (newCapacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than 0");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateNodePosition(LruNode node)
    {
        _list.Remove(node);
        _list.AddFirst(node);
    }

    public void Add(TKey key, TValue value)
    {
        if (_dictionary.TryGetValue(key, out LruNode? node))
        {
            node.UpdateValue(value);
            UpdateNodePosition(node);
        }
        else
        {
            AddInternal(key, value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddInternal(TKey key, TValue value)
    {
        if (_list.Count == _capacity)
        {
            _dictionary.Remove(_list.Last!.Value.Key);
            _list.RemoveLast();
        }

        LruNode newNode = new (key, value);

        _dictionary.Add(key, newNode);
        _list.AddFirst(newNode);
    }

    public bool Contains(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default (TValue);
        if (_dictionary.TryGetValue(key, out LruNode? node))
        {
            UpdateNodePosition(node);
            value = node.Value;
            return true;
        }

        return false;
    }

    public TValue Get(TKey key)
    {
        if (!TryGetValue(key, out TValue? value)) 
        {
            throw new KeyNotFoundException(key.ToString());
        }
        
        return value;
    }
}
