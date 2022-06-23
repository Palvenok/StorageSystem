using System.Collections.Generic;
using UnityEngine;

public class Storage : InteractableObject
{
    [SerializeField] private float _weightLimit;
    [SerializeField] private bool _isClosed;
    [SerializeField] private int _key;
    [SerializeField] private List<Item> _items = new List<Item>();

    private float _currentWeight;

    public bool IsClosed => _isClosed;
    public int ItemsCount => _items.Count;

    public void OpenStorage(int key)
    {
        if (key == _key) _isClosed = false;
    }

    public List<Item> TakeItemsList()
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
        return true;
    }

    public Item TakeItem(int index)
    {
        if(_items.Count == 0) return null;

        index = Mathf.Clamp(index, 0, _items.Count - 1);
        
        var item = _items[index];
        _items.RemoveAt(index);
        return item;
    }
}
