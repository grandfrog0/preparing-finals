using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemViewDrag : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rect;
    private Image _image;
    private Transform _parent;
    private ItemView _view;

    private int _lastIndex;

    public int CurrentInventoryIndex;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _view = GetComponent<ItemView>();
        _parent = _rect.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta;
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>();

        dragRect.SetParent(_parent);
        if (dragRect.anchoredPosition.x < _rect.anchoredPosition.x)
        {
            dragRect.SetSiblingIndex(_rect.GetSiblingIndex());
            InventoryViewer.CurrentInventory.MoveBefore(_view.Item, dragRect.GetComponent<ItemView>().Item);
        }
        else
        {
            dragRect.SetSiblingIndex(_rect.GetSiblingIndex() + 1);
            InventoryViewer.CurrentInventory.MoveAfter(_view.Item, dragRect.GetComponent<ItemView>().Item);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.raycastTarget = false;

        _lastIndex = _rect.GetSiblingIndex();

        _rect.SetParent(_parent.parent);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;

        if (_rect.parent != _parent) // если не попал по второму предмету
        {
            _rect.SetParent(_parent);
            _rect.SetSiblingIndex(_lastIndex);
        }
    }
}
