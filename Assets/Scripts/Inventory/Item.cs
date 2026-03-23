using UnityEngine;

public class Item : ScriptableObject
{
    public string ID;
    public string Title;
    public Sprite Icon;

    public override string ToString() => Title;
}
