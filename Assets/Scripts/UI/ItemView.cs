using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text text;
    [SerializeField] Button button;
    public Item Item { get; private set; }

    public void Init(Item item, UnityAction onClick)
    {
        Item = item;

        image.sprite = item.Icon;
        text.text = item.Title;
        button.onClick.AddListener(onClick);
    }
}
