using UnityEngine;

[CreateAssetMenu(fileName = "Materials Config", menuName = "Config/Material Config")]
public class MaterialsConfig : ScriptableObject
{
    [SerializeField] private Material[] _blockedMaterials;
    [SerializeField] private Material[] _unblockedMaterials;

    public Material[] BlockedMaterials => _blockedMaterials;
    public Material[] UnblockedMaterials => _unblockedMaterials;
}
