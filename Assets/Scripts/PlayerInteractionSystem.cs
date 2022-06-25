using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractionSystem : MonoBehaviour
{
    [HideInInspector] public UnityEvent<GameObject> OnCanInteract;
    [HideInInspector] public UnityEvent<Vector3> OnCanInteractPosition;

    [SerializeField] private float _interactDistance;
    [SerializeField] private Camera _camera;
    [SerializeField] private Storage _playerInventory;
    [SerializeField] private UIController _uiController;
    [Space]
    [SerializeField] private float _holdTime = 2f;

    private float _timer;
    private Coroutine _timerCor;
    private bool _holdInteract;
    private bool _unHolded;
    private GameObject _movableTarget;

    private void Update()
    {
        var target = RayCast();
        OnCanInteract?.Invoke(target.gameObject);
        OnCanInteractPosition?.Invoke(target.point);
        if (Input.GetKeyDown(KeyCode.Tab)) ChekInventory();
        if (Input.GetKeyDown(KeyCode.E)) _timerCor = StartCoroutine(Timer(target.gameObject));
        if (Input.GetKeyUp(KeyCode.E))
        {
            StopCoroutine(_timerCor);
            if (_unHolded)
            {
                _holdInteract = false;
                _unHolded = false;
                return;
            }
            if (_holdInteract) return;
            _timer = 0;
            Interact(target.gameObject);
        }
    }

    private IEnumerator Timer(GameObject target)
    {
        _timer = 0;
        while (_timer <= _holdTime)
        {
            yield return null;
            _timer += Time.deltaTime;
        }

        if (_movableTarget != null)
        {
            Interact(_movableTarget);
            yield break;
        }

        Interact(target);
    }

    private void Interact(GameObject target)
    {
        if (target == null) return;

        InteractableObject interactObject;
        if (!target.TryGetComponent(out interactObject)) return;
        
        if (_timer >= _holdTime)
        {
            if (!interactObject.IsMovable) return;

            if (!interactObject.IsMove)
            {
                interactObject.IsMove = true;
                _holdInteract = true;
                OnCanInteractPosition.AddListener(interactObject.MoveToPoint);
                _movableTarget = target;
            }
            else
            {
                if (!interactObject.CanPlace) return;

                interactObject.IsMove = false;
                _unHolded = true;
                OnCanInteractPosition.RemoveAllListeners();
                _movableTarget = null;
            }
            return;
        }

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

    private (GameObject gameObject, Vector3 point) RayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(ray, out hit, _interactDistance))
        {
            return (hit.collider.gameObject, hit.point);
        }

        return (null, _camera.transform.position + _camera.transform.forward * _interactDistance);
    }

    private void OnDestroy()
    {
        OnCanInteract.RemoveAllListeners();
        OnCanInteractPosition.RemoveAllListeners();
    }
}
