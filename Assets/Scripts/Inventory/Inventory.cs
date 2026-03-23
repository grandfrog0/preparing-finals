using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> _items = new();

    public void Add(Item item)
    {
        _items.Add(item);
        Debug.Log($"Item added: {item}. Inventory: {this}");
    }

    public void Replace(int index1, int index2)
    {
        Debug.Log("Before replace: " + this);
        (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
        Debug.Log("After replace: " + this);
    }

    public override string ToString()
    {
        return string.Join("; ", _items);
    }
}
