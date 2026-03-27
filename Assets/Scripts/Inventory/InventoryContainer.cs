using UnityEngine;

public class InventoryContainer : MonoBehaviour
{
    private Entity _entity;
    private EntityMovement _movement;
    private EntityShooter _shooter;

    public Inventory Inventory { get; private set; }

    public void InitInventory(Inventory inventory)
    {
        Inventory = inventory;
    }

    public void UseItem(Item item)
    {
        switch (item.Type)
        {
            case ItemType.Apple or ItemType.Potion:
                _entity.Heal(item.Value);
                break;

            case ItemType.Boots:
                _movement.MovementSpeedMultiplier = item.Value;
                break;

            case ItemType.Crossbow:
                _shooter.Charge((int)item.Value);
                break;
        }

        Inventory.Remove(item);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<DroppedItem>().Item;
            Inventory.Add(item);
            Destroy(other.gameObject);
        }
    }

    private void Start()
    {
        _entity = GetComponent<Entity>();
        _movement = GetComponent<EntityMovement>();
        _shooter = GetComponent<EntityShooter>();
    }

    private void Awake()
    {
        if (Inventory == null)
        {
            InitInventory(new Inventory());
        }
    }
}
