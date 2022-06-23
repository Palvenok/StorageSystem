using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _model;
    [SerializeField] private Transform _head;
    [Space]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _mouseSense = 1f;
    [SerializeField] private float _jumpHeight = 20f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpBoost = 1.1f;
    [SerializeField] private LayerMask _groundLayer = default;
    [Space]
    [SerializeField] private UIController _uiController;

    private float _cachedJumpBoost;
    private bool _charGrounded;
    private Vector3 playerVelocity;
    private CharacterController _character;
    private Vector2 _curRot;
    private float _charHeight;

    private void Start()
    {
        _character = GetComponent<CharacterController>();
        _cachedJumpBoost = _jumpBoost;
        _charHeight = _character.height * .51f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_uiController.PlayerInventoryShown || _uiController.StorageInventoryShown)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        _charGrounded = IsGrounded();
        Move();
        Rotate();
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        _camera.transform.position = _head.position - Vector3.up * .1f;
    }

    private void Move()
    {
        float inputForward = Input.GetAxis("Vertical");
        float inputSide = Input.GetAxis("Horizontal");

        if (_charGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        Vector3 move = (forward * _jumpBoost * inputForward + right * inputSide);

        if (_charGrounded && Input.GetButtonDown("Jump"))
        {
            _charGrounded = false;

            if (inputSide != 0)
                _jumpBoost += _jumpBoost * .1f;
            _jumpBoost = Mathf.Clamp(_jumpBoost, 1f, 10f);

            StartCoroutine(Jump(move));
        }


        if (inputForward <= 0 && _charGrounded)
            _jumpBoost = _cachedJumpBoost;


        playerVelocity.y += _gravity * Time.deltaTime;

        _character.Move(move * _moveSpeed * Time.deltaTime);
        _character.Move(playerVelocity * Time.deltaTime); //vertical velocity
    }

    private IEnumerator Jump(Vector3 vec)
    {
        if (vec == Vector3.zero)
            yield return new WaitForSeconds(0.2f);
        yield return null;
        playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3f * _gravity);
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _curRot.x += mouseX * _mouseSense;
        _curRot.y -= mouseY * _mouseSense;
        _curRot.x = Mathf.Repeat(_curRot.x, 360);
        _curRot.y = Mathf.Clamp(_curRot.y, -80, 80);

        _camera.transform.localRotation = Quaternion.Euler(_curRot.y, 0, 0);
        transform.eulerAngles = new Vector3(0, _curRot.x, 0f);
    }
    
    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _charHeight, _groundLayer))
            return true;

        return false;
    }
    
}
