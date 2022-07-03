using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : Item
{
    [HideInInspector] public UnityEvent OnStorageUpdate;

    [Header("Storage Settings")]
    [SerializeField] private float _weightLimit;
    [SerializeField] private bool _isClosed;
    [SerializeField] private int _key;
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private Transform _dropPoint;

    private float _currentWeight;
    private Storage _otherStorage;

    public bool IsClosed => _isClosed;
    public int ItemsCount => _items.Count;
    public float WeightLimit => _weightLimit;
    public float CurrentWeight => _currentWeight;
    public Storage OtherStorage
    {
        get => _otherStorage;
        set => _otherStorage = value;
    }

    private void Start()
    {
        if (_items.Count != 0)
            for (int i = 0; i < _items.Count; i++)
                {
                    var newItem = Instantiate(_items[i]).GetComponent<Item>();
                    _currentWeight += newItem.Weight;
                    newItem.gameObject.SetActive(false);
                    _items[i] = newItem;
                }
    }

    public void OpenStorage(int key)
    {
        if (key == _key) _isClosed = false;
    }

    public List<Item> GetItemsList()
    {
        if (!_isClosed)
            return _items;
        return null;
    }

    public bool TryPutItem(Item item)
    {
        if (item.Weight + _currentWeight > _weightLimit)
            return false;

        _items.Add(item);
        _currentWeight += item.Weight;
        OnStorageUpdate?.Invoke();
        return true;
    }

    public void TakeItem(Item item)
    {
        if (!_items.Contains(item)) return;

        _currentWeight -= item.Weight;

        _items.Remove(item);
        OnStorageUpdate?.Invoke();
    }

    public void DropItem(Item item)
    {
        if (!_items.Contains(item)) return;

        _currentWeight -= item.Weight;

        item?.Drop(_dropPoint);
        _items.Remove(item);
        OnStorageUpdate?.Invoke();
    }

    public void DropAll()
    {
        foreach (var item in _items)
            item.Drop(_dropPoint);
        _items.Clear();
        OnStorageUpdate?.Invoke();
    }

    public void MoveItemToOtherStorage(Item item)
    {
        if (OtherStorage == null)
        {
            item.Use();
            return;
        }
        if (!_items.Contains(item)) return;

        if (_otherStorage.TryPutItem(item))
            _items.Remove(item);
        else
            return;

        _currentWeight -= item.Weight;

        OnStorageUpdate?.Invoke();
    }

    private void OnDestroy()
    {
        OnStorageUpdate.RemoveAllListeners();
    }
}
