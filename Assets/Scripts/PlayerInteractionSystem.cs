using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractionSystem : MonoBehaviour
{
    [HideInInspector] public UnityEvent<GameObject> OnCanInteract;
    [HideInInspector] public UnityEvent<bool> OnInventoryUsed;

    [SerializeField] private float _interactDistance;
    [SerializeField] private Camera _camera;
    [SerializeField] private Storage _playerInventory;
    [SerializeField] private UIController _uiController;

    private void Update()
    {
        var target = RayCast();
        OnCanInteract?.Invoke(target);
        if (Input.GetKeyDown(KeyCode.E)) Interact(target);
        if (Input.GetKeyDown(KeyCode.Tab)) ChekInventory();
    }

    private void Interact(GameObject target)
    {
        if (target == null) return;
        if (target.TryGetComponent(out Storage storage))
        {
            if (_uiController.StorageInventoryShown) return;

            storage.OtherStorage = _playerInventory;
            _playerInventory.OtherStorage = storage;
            _uiController.ShowStorageInventory(storage);
            _uiController.ShowPlayerInventory();

            return;
        }
        if (target.TryGetComponent(out Item item))
        {
            if(_playerInventory.TryPutItem(item))
                item.PickUp();
            return;
        }
    }

    private void ChekInventory()
    {
        if (!_uiController.PlayerInventoryShown)
            _uiController.ShowPlayerInventory();
        else
        {
            _uiController.HidePlayerInventory();
            _uiController.HideStorageInventory();

            _playerInventory.OtherStorage = null;
        }
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
