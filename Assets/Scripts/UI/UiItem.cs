using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UiItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _label;

    private Item _item;
    private StorageUi _currentStorageUi;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch(eventData.button)
        {
            case PointerEventData.InputButton.Left:
                _currentStorageUi.MoveItem(_item);
                break;
            case PointerEventData.InputButton.Right:
                _currentStorageUi.DropItem(_item);
                break;
        }
    }

    public void SetItem(Item item, StorageUi storageUi)
    {
        _item = item;
        _label.text = item.Name;
        _currentStorageUi = storageUi;
    }

}