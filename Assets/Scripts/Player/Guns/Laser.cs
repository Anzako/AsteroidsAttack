using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MovementController, IPooledObject
{
    [SerializeField] private int numberOfPoints = 10;
    [SerializeField] private LayerMask enemyMask;
    public int damageAmount = 2;

    private float delayedTime = 0f;
    private float collisionCheckTime = 0.1f;
    private List<IDamagable> damagedObjects;

    private LineRenderer lineRenderer;

    // Pooled object
    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        damagedObjects = new List<IDamagable>();
        toGroundPotential = 0.5f;
        aboveGroundDistance = 0.3f;
    }

    protected override void Update()
    {
        delayedTime += Time.deltaTime;
        if (delayedTime < collisionCheckTime) 
        {
            CheckCollision();
        }
    }

    protected override void Start()
    {
        
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
            Move(moveDistance);
            for (int j = 0; j < 3; j++)
            {
                RotateToSurface2();
            }
            lineRenderer.SetPosition(i, transform.position);
        }
    }

    private void CheckCollision()
    {
        for (int i = 0; i < numberOfPoints - 1; i++) 
        {
            RaycastHit hit;
            if (Physics.Linecast(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1), out hit, enemyMask))
            {
                IDamagable damagable = hit.collider.gameObject.GetComponentInParent<IDamagable>();
                if (damagedObjects.Contains(damagable)) return;

                damagedObjects.Add(damagable);
                damagable.Damage(damageAmount);
                Debug.Log(hit.transform.gameObject.name);
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
