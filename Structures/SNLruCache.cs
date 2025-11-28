using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Structures;

public class SNLruCache<TKey, TValue> where TKey : notnull
{
    class Node
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public int Generation { get; private set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateValue(TValue newValue)
        {
            Value = newValue;
        }

        public void UpdateGeneration(int generation)
        {
            Generation = generation;
        }
    }

    private readonly LinkedList<Node>[] _generations;
    private readonly Dictionary<TKey, Node> _mapKeyValue;

    private readonly int _numberOfGenerations;
    private readonly int _maxGenerationNumber;
    public int NumberOfGenerations => _numberOfGenerations;

    private int _capacityPerGeneration;
    public int CapacityPerGeneration => _capacityPerGeneration;


    public SNLruCache(int numberOfGenerations, int capacityPerGeneration)
    {
        EnsureCorrectCapacity(ref capacityPerGeneration);
        EnsureCorrectNumber(ref numberOfGenerations);

        _numberOfGenerations = numberOfGenerations;
        _capacityPerGeneration = capacityPerGeneration;
        _mapKeyValue = new Dictionary<TKey, Node>(numberOfGenerations * capacityPerGeneration);


        _generations = new LinkedList<Node>[numberOfGenerations];
        for (int i = 0; i < numberOfGenerations; i++)
        {
            _generations[i] = new LinkedList<Node>();
        }

        _maxGenerationNumber = numberOfGenerations - 1;
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

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (!_mapKeyValue.TryGetValue(key, out Node? node))
        {
            value = default;
            return false;
        }
        
        UpdateNodePosition(node);

        value = node.Value;
        return true;
    }

    
    public void Put(TKey key, TValue value)
    {
        if (_mapKeyValue.TryGetValue(key, out Node? node))
        {
            node.UpdateValue(value);
            SwapNodeInGeneration(node);
        }
        else
        {
            node = new Node(key, value);
            PromoteToGeneration(node, node.Generation);
        }
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    private void UpdateNodePosition(Node node)
    {
        if (node.Generation < _maxGenerationNumber)
        {
            RemoveFromGeneration(node, node.Generation);
            
            PromoteToGeneration(node, node.Generation + 1);
        }
        else
        {
            SwapNodeInGeneration(node);
        }
    }

    private void SwapNodeInGeneration(Node node)
    {
        LinkedList<Node> generation = _generations[node.Generation];
        generation.Remove(node);
        generation.AddFirst(node);
    }

    private void RemoveFromGeneration(Node node, int generation)
    {
        _generations[generation].Remove(node);
    }
    private void PromoteToGeneration(Node node, int generation)
    {
        node.UpdateGeneration(generation);
        
        LinkedList<Node> nextGeneration = _generations[generation];
        if (nextGeneration.Count == _capacityPerGeneration)
        {
            FreeGenerationForNew(nextGeneration);
        }

        nextGeneration.AddFirst(node);
    }

    private void FreeGenerationForNew(LinkedList<Node> generation)
    {
        Node last = generation.Last();
        _mapKeyValue.Remove(last.Key);
        generation.RemoveLast();
    }



    /// <summary>
    /// Ensure the capacity is greater than 0.
    /// </summary>
    /// <param name="newCapacity"></param>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCorrectCapacity(ref int newCapacity)
    {
        if (newCapacity <= 0)
            throw new ArgumentException("Capacity must be greater than 0");
    }

    /// <summary>
    /// Ensures that the provided bucket count is at least 2.
    /// </summary>
    /// <param name="bucketCount">The bucket count to validate.</param>
    /// <exception cref="ArgumentException">Thrown if the bucket count is less than 2.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCorrectNumber(ref int bucketCount)
    {
        if (bucketCount < 2)
            throw new ArgumentException("Bucket number must be at least 2");
    }
}