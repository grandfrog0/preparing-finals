using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item Item;

    private void Start()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = Item.Icon;
    }
}
