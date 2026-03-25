using NUnit.Framework;
using UnityEngine;

public class InventoryViewer : MonoBehaviour
{
    [SerializeField] InventoryContainer inventoryContainer;
    private Inventory _currentInventory;

    [SerializeField] ItemView itemViewPrefab;
    [SerializeField] Transform itemViewParent;

    private void Awake()
    {
        _currentInventory = inventoryContainer.Inventory;
        _currentInventory.OnRefresh += Refresh;
    }

    private void Refresh()
    {
        Clear();

        foreach (Item item in _currentInventory)
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
