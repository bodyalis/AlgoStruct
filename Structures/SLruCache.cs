using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Structures;

public class SLruCache<TKey, TValue> where TKey : notnull
{
    class Node
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateValue(TValue newValue)
        {
            Value = newValue;
        }
    }

    private readonly LinkedList<Node> _probationList;
    private readonly Dictionary<TKey, Node> _probationKeyValueMap;

    private readonly LinkedList<Node> _protectedList;
    private readonly Dictionary<TKey, Node> _protectedKeyValueMap;

    private int _capacity;
    public int Capacity => _capacity;
    public int ProtectedCount => _protectedList.Count;
    public int ProbationCount => _probationList.Count;
    public int Count => ProtectedCount + ProbationCount;

    public SLruCache(int capacity)
    {
        EnsureCorrectCapacity(capacity);

        _capacity = capacity;

        _protectedKeyValueMap = new Dictionary<TKey, Node>(capacity);
        _protectedList = new LinkedList<Node>();

        _probationList = new LinkedList<Node>();
        _probationKeyValueMap = new Dictionary<TKey, Node>(capacity);
    }

    /// <summary>
    /// Get the value of the key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public TValue Get(TKey key)
    {
        if (!TryGetValue(key, out TValue? value))
        {
            throw new KeyNotFoundException(key.ToString());
        }
        
        return value;
    }

    /// <summary>
    /// Try to get the value of the key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (_protectedKeyValueMap.TryGetValue(key, out Node? node))
        {
            UpdateNodePositionInList(node, _protectedList);
            value = node.Value;
            return true;
        }

        if (_probationKeyValueMap.Remove(key, out node))
        {
            _probationList.Remove(node);

            AddNodeInternal(node, _protectedList, _protectedKeyValueMap);

            value = node.Value;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Add or update the value of the key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Put(TKey key, TValue value)
    {
        if (_protectedKeyValueMap.TryGetValue(key, out Node? node))
        {
            UpdateNodePositionInList(node, _protectedList);
            node.UpdateValue(value);
        }
        else if (_probationKeyValueMap.Remove(key, out node))
        {
            _probationList.Remove(node);

            node.UpdateValue(value);

            AddNodeInternal(node, _protectedList, _protectedKeyValueMap);
        }
        else
        {
            node = new Node(key, value);

            AddNodeInternal(node, _probationList, _probationKeyValueMap);
        }
    }

    /// <summary>
    /// Add the node to the list.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="list"></param>
    /// <param name="keyValueMap"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddNodeInternal(Node node, LinkedList<Node> list, Dictionary<TKey, Node> keyValueMap)
    {
        if (keyValueMap.Count >= _capacity)
        {
            keyValueMap.Remove(list.Last!.Value.Key);
            list.RemoveLast();
        }

        list.AddFirst(node);
        keyValueMap.Add(node.Key, node);
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
            while (_protectedList.Count > newCapacity)
            {
                Node lastNode = _protectedList.Last!.Value;
                _protectedKeyValueMap.Remove(lastNode.Key);
                _protectedList.RemoveLast();
            }

            while (_probationList.Count > newCapacity)
            {
                Node lastNode = _probationList.Last!.Value;
                _probationKeyValueMap.Remove(lastNode.Key);
                _probationList.RemoveLast();
            }
        }

        _capacity = newCapacity;
    }

    /// <summary>
    /// Ensure the capacity is greater than 0.
    /// </summary>
    /// <param name="newCapacity"></param>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCorrectCapacity(int newCapacity)
    {
        if (newCapacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than 0");
        }
    }
    
    /// <summary>
    /// Update the position of the node in the list. Remove the node from the list and add it to the head.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="list"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateNodePositionInList(Node node, LinkedList<Node> list)
    {
        list.Remove(node);
        list.AddFirst(node);
    }
}
