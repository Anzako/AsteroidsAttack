using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IPooledObject
{
    // Visualise things
    private LineRenderer lineRenderer;
    [SerializeField] private int numberOfPoints = 10;
    public float aboveGroundDistance = 0;
    private Vector2 directionVector = new Vector2(0, 1);

    // Laser variables
    public int damageAmount = 2;
    [SerializeField] private LayerMask enemyMask;

    private float delayedTime = 0f;
    private float collisionCheckTime = 0.1f;
    private List<IDamagable> damagedObjects;

    // Sounds
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip laserShootSoundClip;

    // Pooled object
    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        damagedObjects = new List<IDamagable>();
    }

    private void Update()
    {
        delayedTime += Time.deltaTime;
        if (delayedTime < collisionCheckTime) 
        {
            CheckCollision();
        }
    }

    private void OnEnable()
    {
        ShootLaser();

        StartCoroutine(DestroyAfterSpawn());
    }

    private void ShootLaser()
    {
        float moveDistance = 1f;
        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 1; i < numberOfPoints; i++)
        {
            MovementController.Move(transform, directionVector, moveDistance);
            for (int j = 0; j < 3; j++)
            {
                MovementController.RotateToSurface(transform, aboveGroundDistance);
            }
            lineRenderer.SetPosition(i, transform.position);
        }

        SoundFXManager.Instance.PlaySoundFXClip(laserShootSoundClip, transform, 1f);
    }

    private void CheckCollision()
    {
        for (int i = 0; i < numberOfPoints - 1; i++) 
        {
            if (Physics.Linecast(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1), out RaycastHit hit, enemyMask))
            {
                IDamagable damagable = hit.collider.gameObject.GetComponentInParent<IDamagable>();
                if (damagedObjects.Contains(damagable)) return;

                damagedObjects.Add(damagable);
                SoundFXManager.Instance.PlaySoundFXClip(hitSoundClip, transform, 1f);
                damagable.Damage(damageAmount);
            }
        }
    }

    private IEnumerator DestroyAfterSpawn()
    {
        yield return new WaitForSeconds(0.3f);
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
    }

    public void OnObjectSpawn()
    {
        delayedTime = 0f;
        damagedObjects = new List<IDamagable>();
    }
}
