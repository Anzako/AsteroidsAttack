using UnityEngine;

public class HealUpAsteroid : AsteroidController, IDropable
{
    [SerializeField] private GameObject dropableObject;
    
    public GameObject DropableObject { get { return dropableObject; } }

    public void Drop()
    {
        Instantiate(DropableObject, transform.position, transform.rotation);
    }

    protected override void Destroy()
    {
        base.Destroy();
        Drop();
    }
}
