using UnityEngine;

public class Item : InteractableObject
{
    [Header("Item settings")]
    [SerializeField] private string _name;
    [SerializeField] private float _weight;

    private Rigidbody _rb;

    public string Name => _name;
    public float Weight => _weight;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public void Drop(Transform dropPoint)
    {
        transform.position = dropPoint.position;
        transform.rotation = dropPoint.rotation;

        gameObject.SetActive(true);
        _rb.AddForce(transform.forward);
    }
}
