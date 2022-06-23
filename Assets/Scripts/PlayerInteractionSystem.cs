using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractionSystem : MonoBehaviour
{
    [HideInInspector] public UnityEvent<GameObject> OnCanInteract;

    [SerializeField] private float _interactDistance;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private Storage _playerInventory;

    private void Update()
    {
        var target = RayCast();
        OnCanInteract?.Invoke(target);
        if (Input.GetKeyDown(KeyCode.E)) Interact(target);
        if (Input.GetKeyDown(KeyCode.Q)) DropFromInventory();
    }

    private void Interact(GameObject target)
    {
        if (target == null) return;
        if (target.TryGetComponent(out Storage storage))
        {
            var list = storage.TakeItemsList();
            foreach (var i in list)
            {
                Debug.Log(i.Name);
            }
            return;
        }
        if (target.TryGetComponent(out Item item))
        {
            if(_playerInventory.TryPutItem(item))
                item.PickUp();
            return;
        }
    }

    private void DropFromInventory()
    {
        int itemIndex = _playerInventory.ItemsCount - 1;
        var item = _playerInventory.TakeItem(itemIndex);
        item?.Drop(_dropPoint);
    }

    private GameObject RayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(ray, out hit, _interactDistance))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    private void OnDestroy()
    {
        OnCanInteract.RemoveAllListeners();
    }
}
