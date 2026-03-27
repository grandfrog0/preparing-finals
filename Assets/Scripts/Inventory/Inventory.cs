using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : IEnumerable
{
    public event Action OnRefresh;

    private List<Item> _items = new();

    public int Count => _items.Count;

    public void MoveBefore(Item point, Item target)
    {
        _items.Remove(target);
        _items.Insert(_items.IndexOf(point), target);

        OnRefresh?.Invoke();
        Debug.Log(_items.ToSeparatedString(", "));
    }
    public void MoveAfter(Item point, Item target)
    {
        _items.Remove(target);
        _items.Insert(_items.IndexOf(point) + 1, target);

        OnRefresh?.Invoke();
    }

    public Item Get(int index)
    {
        return _items[index];
    }

    public void Add(Item item)
    {
        _items.Add(item);
        OnRefresh?.Invoke();
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
        OnRefresh?.Invoke();
    }

    public void Clear()
    {
        _items.Clear();
        OnRefresh?.Invoke();
    }

    public void Replace(int index1, int index2)
    {
        (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
        OnRefresh?.Invoke();
    }

    public override string ToString()
    {
        return string.Join("; ", _items);
    }

    public IEnumerator GetEnumerator()
        => _items.GetEnumerator();
}
