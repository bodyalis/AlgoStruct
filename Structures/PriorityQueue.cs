namespace Structures;

public class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private readonly Lazy<List<(TElement Value, TPriority Priority)>> _queue = new ();
    private readonly IComparer<TPriority> _comparer;
    public int Count => _queue.Value.Count;

    public PriorityQueue(IComparer<TPriority> priorityComparer)
    {
        _comparer = priorityComparer ?? Comparer<TPriority>.Default;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        _queue.Value.Add((element, priority));
        HeapifyUp(_queue.Value.Count - 1);
    }

    public TElement Dequeue()
    {
        if (_queue.Value.Count == 0)
        {
            throw new IndexOutOfRangeException();
        }

        (TElement Value, TPriority Priority) element = _queue.Value[0];

        _queue.Value[0] = _queue.Value[^1];
        _queue.Value.RemoveAt(_queue.Value.Count - 1);

        if (_queue.Value.Count > 0)
        {
            HeapifyDown(0);
        }

        return element.Value;
    }

    public bool TryDequeue(out TElement element, out TPriority priority)
    {
        if (_queue.Value.Count == 0)
        {
            element = default (TElement);
            priority = default (TPriority);
            return false;
        }

        (element, priority) = _queue.Value[0];

        _queue.Value[0] = _queue.Value[^1];
        _queue.Value.RemoveAt(_queue.Value.Count - 1);

        if (_queue.Value.Count > 0)
        {
            HeapifyDown(0);
        }

        return true;
    }

    public TElement Peek()
    {
        if (_queue.Value.Count == 0)
        {
            throw new InvalidOperationException("Очередь пуста");
        }

        return _queue.Value[0].Value;
    }

    public bool TryPeek(out TElement element, out TPriority priority)
    {
        if (_queue.Value.Count == 0)
        {
            element = default (TElement);
            priority = default (TPriority);
            return false;
        }

        (element, priority) = _queue.Value[0];
        return true;
    }

    // l => i * 2 + 1
    // r => i * 2 + 2
    // p => (i - 1) / 2 (на цело делить)

    private void HeapifyUp(int index)
    {
        // Поднимаем пока будет больше чем родительский

        while (index > 0)
        {
            int parentIdx = (index - 1) / 2;

            if (ComparePriority(index, parentIdx) >= 0)
            {
                break;
            }

            Swap(index, parentIdx);
            index = parentIdx;
        }

    }

    private int ComparePriority(int idx1, int idx2)
    {
        TPriority c1 = _queue.Value[idx1].Priority;
        TPriority c2 = _queue.Value[idx2].Priority;
        int res = _comparer.Compare(c1, c2);

        return res;
    }

    // Опускаем пока приоритет родителя меньше приоритета 
    private void HeapifyDown(int index)
    {

        while (true)
        {

            int leftChildIndex = index * 2 + 1;
            int rightChildIndex = index * 2 + 2;
            int smallestIndex = index;

            if (leftChildIndex < _queue.Value.Count
                && ComparePriority(leftChildIndex, smallestIndex) < 0)
            {
                smallestIndex = leftChildIndex;
            }

            if (rightChildIndex < _queue.Value.Count
                && ComparePriority(rightChildIndex, smallestIndex) < 0)
            {
                smallestIndex = rightChildIndex;
            }

            if (smallestIndex == index)
            {
                break;
            }

            Swap(index, smallestIndex);
            index = smallestIndex;
        }


    }

    private void Swap(int idx1, int idx2)
    {
        (_queue.Value[idx1], _queue.Value[idx2]) = (_queue.Value[idx2], _queue.Value[idx1]);
    }
}
public interface IPriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    /// <summary>
    /// Добавление элемента с приоритетом
    /// </summary>
    /// <param name="element"></param>
    /// <param name="priority"></param>
    public void Enqueue(TElement element, TPriority priority);

    /// <summary>
    /// Извлечение элемента с наивысшим приоритетом (минимальным значением)
    /// </summary>
    /// <returns></returns>
    public TElement Dequeue();

    /// <summary>
    /// Извлечение элемента с наивысшим приоритетом (минимальным значением)
    /// </summary>
    /// <param name="element"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
    public bool TryDequeue(out TElement element, out TPriority priority);

    /// <summary>
    /// Просмотр элемента с наивысшим приоритетом (минимальным значением)
    /// </summary>
    /// <returns></returns>
    public TElement Peek();

    /// <summary>
    /// Просмотр элемента с наивысшим приоритетом (минимальным значением)
    /// </summary>
    /// <param name="element"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
    public bool TryPeek(out TElement element, out TPriority priority);
}
