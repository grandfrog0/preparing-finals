using System.Collections.Generic;
using UnityEngine;

public class InventoryViewer : MonoBehaviour
{
    public static Inventory CurrentInventory { get; private set; }

    [SerializeField] InventoryContainer inventoryContainer;

    [SerializeField] ItemView itemViewPrefab;
    [SerializeField] Transform itemViewParent;

    private void Start()
    {
        CurrentInventory = inventoryContainer.Inventory;
        Refresh();
        CurrentInventory.OnRefresh += Refresh;
    }

    private void Refresh()
    {
        Clear();

        foreach (Item item in CurrentInventory)
        {
            ItemView view = Instantiate(itemViewPrefab, itemViewParent);

            Item currentItem = item;
            view.Init(currentItem, () => inventoryContainer.UseItem(currentItem));
        }
    }

    private void Clear()
    {
        for (int i = 0; i < itemViewParent.childCount; i++)
        {
            Destroy(itemViewParent.GetChild(i).gameObject);
        }
    }
}
