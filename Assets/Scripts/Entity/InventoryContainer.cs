using UnityEngine;

public class InventoryContainer : MonoBehaviour
{
    public Inventory Inventory = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<DroppedItem>().Item;
            Inventory.Add(item);
            Destroy(other.gameObject);
        }
    }
}
