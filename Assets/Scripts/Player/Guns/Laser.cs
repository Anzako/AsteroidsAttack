using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Visualise things
    private LineRenderer lineRenderer;
    private Material material;
    [SerializeField] private int numberOfPoints = 10;
    public float aboveGroundDistance = 0.2f;
    private Vector2 directionVector = new Vector2(0, 1);
    private int dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    [SerializeField] private float dissolveTime = .2f;

    // Laser variables
    public int damageAmount = 2;
    [SerializeField] private LayerMask enemyMask;
    private List<IDamagable> damagedObjects;
    [SerializeField] private poolTags particleTag;

    // Sounds
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip laserShootSoundClip;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        damagedObjects = new List<IDamagable>();
        material = lineRenderer.materials[0];
    }

    private void Update()
    {
        RenderLaser();
    }

    private void OnEnable()
    {
        RenderLaser();
        ShootLaser();
    }

    private void ShootLaser()
    {
        CheckCollision();
        SoundFXManager.Instance.PlaySoundFXClip(laserShootSoundClip, transform, 1f);

        StartCoroutine(DestroyAfterSpawn());
    }

    private void RenderLaser()
    {
        float moveDistance = 1f;
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;

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
                ObjectPooler.Instance.SpawnObject(particleTag, hit.collider.transform.position, hit.collider.transform.rotation);
            }
        }
    }

    private IEnumerator DestroyAfterSpawn()
    {
        float elapsedTime = 0f;
        

        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1, (elapsedTime / dissolveTime));

            material.SetFloat(dissolveAmount, lerpedDissolve);

            yield return null;
        }

        Destroy(gameObject);
    }

    public void OnObjectSpawn()
    {
        damagedObjects = new List<IDamagable>();
    }
}
