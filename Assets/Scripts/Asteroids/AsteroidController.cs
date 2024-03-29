using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AsteroidController : MonoBehaviour, IPooledObject
{
    // Movement
    private MovementController mController;
    private Vector2 direction;

    // Pooled object
    private string tag;
    public string Tag
    {
        get { return tag; }
        set { tag = value; }
    }

    // Health Controller
    private AsteroidsHealth hController;

    private void Awake()
    {
        mController = GetComponent<MovementController>();
        Tag = "asteroid";
    }

    private void FixedUpdate()
    {
        mController.MovementFixedUpdate(direction);
    }

    public void OnObjectSpawn()
    {
        direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }

    public void OnObjectPooled()
    {
        Debug.Log("Object Pooled");
        ObjectPooler.instance.ReturnObjectToPool(this.gameObject);

        AsteroidsSpawner.instance.SpawnAsteroidInTime(2);
    }

}
