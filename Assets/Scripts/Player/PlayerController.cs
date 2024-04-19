using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController mController;
    [SerializeField] private InputController inputController;
    public int actualScore = 0;

    Vector2 direction = Vector2.zero;

    // Shooting
    public string objectTag;
    private float lastShootTime = 0;
    public float timeToShoot;

    // Update is called once per frame
    void Update()
    {
        mController.MovementUpdate(direction);
        mController.PlayerMouseUpdate(inputController.mousePos);
        lastShootTime += Time.deltaTime;
    }

    public void ShootProjectile()
    {
        if (lastShootTime >= timeToShoot)
        {
            Vector3 spawnPosition = transform.position + transform.forward.normalized;
            ObjectPooler.instance.SpawnObject(objectTag, spawnPosition, transform.rotation);
            lastShootTime = 0f;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

}
