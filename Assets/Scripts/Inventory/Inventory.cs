using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IEnumerable
{
    public event Action OnRefresh;

    private List<Item> _items = new();

    public void Add(Item item)
    {
        _items.Add(item);
        OnRefresh.Invoke();
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
        OnRefresh.Invoke();
    }

    public void Replace(int index1, int index2)
    {
        (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
        OnRefresh.Invoke();
    }

    public override string ToString()
    {
        return string.Join("; ", _items);
    }

    public IEnumerator GetEnumerator()
        => _items.GetEnumerator();
}
