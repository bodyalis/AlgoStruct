using System.Diagnostics.CodeAnalysis;

namespace Structures;

public class PriorityQueue<TElement, TPriority>
    where TPriority : IComparable<TPriority>
{
    private readonly IComparer<TPriority> _comparer;
    private readonly List<(TElement Element, TPriority Priority)> _elements = new List<(TElement, TPriority)>();
    
    // parent = (i - 1) / 2
    // left = 2 * i + 1;
    // right = 2 * i + 2;

    public int Count => _elements.Count;
    
    public PriorityQueue(IComparer<TPriority>? comparer = null)
    {
        _comparer = comparer ?? Comparer<TPriority>.Default;
    }
    
    private void HeapifyUp(int index)
    {
        while (index >= 1)
        {
            int parent = (index - 1) / 2;

            if (_comparer.Compare(_elements[index].Priority, _elements[parent].Priority) > 0)
            {
                break;
            }
            
            (_elements[index], _elements[parent]) = (_elements[parent], _elements[index]);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int smallestIndex = index;
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChind = 2 * index + 2;

            if (leftChild < _elements.Count
                && _comparer.Compare(_elements[leftChild].Priority, _elements[smallestIndex].Priority) < 0)
            {
                smallestIndex = leftChild;
            }

            if (rightChind < _elements.Count
                && _comparer.Compare(_elements[rightChind].Priority, _elements[smallestIndex].Priority) < 0)
            {
                smallestIndex = rightChind;
            }
            
            if (index == smallestIndex)
            {
                break;
            }
            
            (_elements[index], _elements[smallestIndex]) = (_elements[smallestIndex], _elements[index]);
            index = smallestIndex;
        }
    }
    
    public void Enqueue(TElement element, TPriority priority)
    {
        _elements.Add((element, priority));
        HeapifyUp(_elements.Count - 1);
    }

    public (TElement Element, TPriority Priority) Dequeue()
    {
        if (_elements.Count == 0)
        {
            throw new Exception("Empty queue");
        }        
        
        var result = _elements[0];
        
        _elements[0] = _elements[^1];
        _elements.RemoveAt(_elements.Count - 1);

        if (_elements.Count > 1)
        {
            HeapifyDown(0);
        }

        return result;
    }

    public bool TryDequeue([MaybeNullWhen(false)] out TElement element,[MaybeNullWhen(false)] out TPriority priority)
    {
        element = default;
        priority = default;
        if (_elements.Count == 0)
        {
            return false;
        }

        (element, priority) = Dequeue();
        return true;
    }

    public (TElement, TPriority) Peek()
    {
        if (_elements.Count == 0)
        {
            throw new Exception("Empty queue");
        }
        
        return _elements[0];
    }

    public bool TryPeek([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
    {
        element  = default;
        priority = default;
        if (_elements.Count == 0)
        {
            return false;
        }
        
        (element, priority) = Peek();
        return true;
    }
}