using UnityEngine;

public interface IDropable
{
    GameObject DropableObject { get; }

    public void Drop();
}
