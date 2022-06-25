using UnityEngine;

public class Item : InteractableObject
{
    [Header("Item settings")]
    [SerializeField] private string _name;
    [SerializeField] private float _weight;

    public string Name => _name;
    public float Weight => _weight;

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public void Drop(Transform dropPoint)
    {
        transform.position = dropPoint.position;
        transform.rotation = dropPoint.rotation;

        transform.parent = null;

        gameObject.SetActive(true);
    }
}
