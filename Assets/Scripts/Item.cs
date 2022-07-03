using UnityEngine;

public abstract class Item : InteractableObject
{
    [Header("Item settings")]
    [SerializeField] private string _name;
    [SerializeField] private float _weight;
    [SerializeField] private bool _placeOnDrop;

    public string Name => _name;
    public float Weight => _weight;

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    public void Drop(Transform dropPoint)
    {
        if(_placeOnDrop)
        {
            var pInterractSystem = FindObjectOfType<PlayerInteractionSystem>();
            pInterractSystem.PlaceItem(gameObject);

            transform.parent = null;

            gameObject.SetActive(true);

            return;
        }

        transform.position = dropPoint.position;
        transform.rotation = dropPoint.rotation;

        transform.parent = null;

        gameObject.SetActive(true);
    }

    public virtual float Use()
    {
        Debug.Log($"{Name} used");
        return 0;
    }
}
