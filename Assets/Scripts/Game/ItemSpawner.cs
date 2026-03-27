using NUnit.Framework;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] InventoryContainer inventoryContainer;
    [SerializeField] GameObject preparedItems;
    private Inventory _checkInventory;
    private GameObject _currentSpawnedItems;

    private void Start()
    {
        _checkInventory = inventoryContainer.Inventory;

        CheckItems();
        _checkInventory.OnRefresh += CheckItems;
    }

    public void CheckItems()
    {
        if (_checkInventory.Count == 0 && (_currentSpawnedItems == null || _currentSpawnedItems.transform.childCount == 0))
        {
            Destroy(_currentSpawnedItems);

            _currentSpawnedItems = Instantiate(preparedItems, preparedItems.transform.parent);
            _currentSpawnedItems.SetActive(true);
        }
    }
}
