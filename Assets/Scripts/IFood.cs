using UnityEngine;

public interface IFood
{
    public float Energy { get; }
    public bool IsEmpty { get; }

    public float Use();
}
