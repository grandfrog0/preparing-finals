using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text text;
    [SerializeField] Button button;
    public void Init(Item item, UnityAction onClick)
    {
        image.sprite = item.Icon;
        text.text = item.Title;
        button.onClick.AddListener(onClick);
    }
}
