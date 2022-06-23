using UnityEngine;

public class Bottle : Item, IFood
{
    [Header("Food settings")]
    [SerializeField] private float _energy = 1;
    [SerializeField] private bool _isEmpty = false;

    public bool IsEmpty => _isEmpty;
    public float Energy => _energy;

    public float Use()
    {
        if (_isEmpty) return 0;

        _isEmpty = true;
        return _energy;
    }
}
