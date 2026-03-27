using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsLibrary : MonoBehaviour
{
    public static ItemsLibrary Instance;

    public List<Item> Items;
    public Item GetItem(ItemType type)
        => Items.First(x => x.Type == type);

    private void Awake()
    {
        Instance = this;
    }
}
