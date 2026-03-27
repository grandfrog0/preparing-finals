using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "SO/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    public List<ItemType> Items;
    private Inventory _inventory;
    public Inventory Inventory
    {
        get
        {
            if (_inventory == null)
            {
                _inventory = new();

                foreach (ItemType type in Items)
                {
                    _inventory.Add(ItemsLibrary.Instance.GetItem(type));
                }

                _inventory.OnRefresh += () => {

                    Items.Clear();
                    foreach (Item item in Inventory)
                    {
                        Items.Add(item.Type);
                    }
                };
            }
            return _inventory;
        }
    }
}
