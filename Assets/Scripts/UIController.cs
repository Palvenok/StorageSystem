using System;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactNote;
    [SerializeField] private PlayerInteractionSystem _interactionSystem;

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
}
