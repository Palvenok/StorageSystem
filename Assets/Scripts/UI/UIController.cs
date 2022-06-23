using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactNote;
    [SerializeField] private PlayerInteractionSystem _interactionSystem;
    [SerializeField] private StorageUi _playerStorageUi;
    [SerializeField] private StorageUi _storageUi;

    private bool _playerInventoryShown;
    private bool _storageInventoryShown;

    public bool PlayerInventoryShown => _playerInventoryShown;
    public bool StorageInventoryShown => _storageInventoryShown;

    private void Start()
    {
        if( _interactionSystem == null ) _interactionSystem = FindObjectOfType<PlayerInteractionSystem>();
        _interactionSystem.OnCanInteract.AddListener(UpdateNote);
    }

    private void UpdateNote(GameObject target)
    {
        _interactNote.text = "";
        if (target == null) return;
        if (target.TryGetComponent<Item>(out Item item)) _interactNote.text = "Pick up\n" + item.Name;
        if (target.GetComponent<Storage>() != null) _interactNote.text = "Open";
    }

    public void ShowPlayerInventory()
    {
        _playerStorageUi.Show();
        _playerInventoryShown = true;
    }

    public void HidePlayerInventory()
    {
        _playerStorageUi.Hide();
        _playerInventoryShown = false;
    }

    public void ShowStorageInventory(Storage storage)
    {
        _storageUi.ConnectedStoraget = storage;
        _storageUi.Show();
        _storageInventoryShown = true;
    }

    public void HideStorageInventory()
    {
        _storageUi.Hide();
        _storageInventoryShown = false;
    }
}
