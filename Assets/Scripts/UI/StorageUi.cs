using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageUi : MonoBehaviour
{
    [SerializeField] private UiItem _uiItem;
    [SerializeField] private Storage _connectedStorage;
    [SerializeField] private Transform _holder;
    [SerializeField] private Image _backGround;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _space;

    private List<UiItem> _uiItems = new List<UiItem>();
    private bool _isHiden = true;

    public Storage ConnectedStoraget
    {
        get { return _connectedStorage; }
        set 
        {
            _connectedStorage?.OnStorageUpdate.RemoveAllListeners();
            _connectedStorage = value;
            _connectedStorage.OnStorageUpdate.AddListener(UpdateUI);
        }
    }

    private void Start()
    {
        _connectedStorage?.OnStorageUpdate.AddListener(UpdateUI);

        _backGround.gameObject.SetActive(false);
        _name.gameObject.SetActive(false);
        _space.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        if (_isHiden) return;

        _name.text = _connectedStorage.Name;
        _space.text = $"Used {_connectedStorage.CurrentWeight}/{_connectedStorage.WeightLimit}";

        foreach (var item in _uiItems)
        {
            Destroy(item.gameObject);
        }
        _uiItems.Clear();

        foreach (var item in _connectedStorage.GetItemsList())
        {
            var uiItem = Instantiate(_uiItem, _holder);
            uiItem.SetItem(item, this);
            _uiItems.Add(uiItem);
        }
    }

    public void Show()
    {

        _isHiden = false;
        _backGround.gameObject.SetActive(true);
        _name.gameObject.SetActive(true);
        _space.gameObject.SetActive(true);
        UpdateUI();
    }

    public void Hide()
    {
        foreach (var item in _uiItems)
        {
            Destroy(item.gameObject);
        }
        _uiItems.Clear();

        _isHiden = true;
        _backGround.gameObject.SetActive(false);
        _name.gameObject.SetActive(false);
        _space.gameObject.SetActive(false);
    }

    public void DropItem(Item item)
    {
        var itemsInStorage = _connectedStorage.GetItemsList();
        if (itemsInStorage.Contains(item)) _connectedStorage.DropItem(item);
    }

    public void MoveItem(Item item)
    {
        var itemsInStorage = _connectedStorage.GetItemsList();
        if (itemsInStorage.Contains(item)) _connectedStorage.MoveItemToOtherStorage(item);
    }
}
