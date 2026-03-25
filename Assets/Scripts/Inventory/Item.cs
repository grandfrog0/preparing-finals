using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "SO/Item")]
public class Item : ScriptableObject
{
    public ItemType Type;
    public string Title;
    public Sprite Icon;

    public float Value;

    public override string ToString() => Title;
}
