using UnityEngine;

public class ReturnParticlesToPool : MonoBehaviour, IPooledObject
{
    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }

    public void OnObjectSpawn()
    {
        
    }

    private void OnParticleSystemStopped()
    {
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }
}
