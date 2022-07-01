using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [Header("Interactable settings")]
    [SerializeField] private bool _isMovable = true;
    [SerializeField] private MaterialsConfig _materialsConfig;
    [SerializeField] private bool _useMeshCenter;

    private bool _isMove;
    private bool _canPlace = true;
    private bool _isBlocked;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private Bounds _bounds;
    private Material[] _defaultMaterial;
    private MeshRenderer _meshRenderer;

    public bool IsMovable => _isMovable;
    public bool CanPlace
    {
        get { return _canPlace; }
        private set
        {
            _canPlace = value;

            if (_canPlace)
                _meshRenderer.materials = _materialsConfig.UnblockedMaterials;
            else
                _meshRenderer.materials = _materialsConfig.BlockedMaterials;
        }
    }
    public bool IsMove
    {
        get { return _isMove; }
        set
        {
            _isMove = value;
            if(_collider)
                _collider.isTrigger = value;

            if(_rigidbody)
                _rigidbody.isKinematic = value;

            if (value)
            { 
                gameObject.layer = 2;
                _meshRenderer.materials = _materialsConfig.UnblockedMaterials;
            }
            else
            {
                gameObject.layer = default;
                _meshRenderer.materials = _defaultMaterial;
            }
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
        _defaultMaterial = _meshRenderer.materials;
    }

    public void MoveToPoint(Vector3 point)
    {
        var newPosition = point;
        var yOffset = _useMeshCenter ? .02f : _bounds.size.y * .51f * transform.localScale.y;
        newPosition.y += yOffset;
        transform.position = newPosition;

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        transform.eulerAngles = Vector3.up;

        CanPlace = false;

        if (Physics.Raycast(ray, out hit, yOffset * 1.1f))
        {
            transform.eulerAngles = hit.normal;
            if(!_isBlocked)
                CanPlace = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsMove) return;
        _isBlocked = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!IsMove) return;
        _isBlocked = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!IsMove) return;
        _isBlocked = false;
    }
}
