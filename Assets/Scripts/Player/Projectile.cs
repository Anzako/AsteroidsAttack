using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private MovementController mController;
    public LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, transform.forward * 2, Color.green);

        mController.MovementFixedUpdate(new Vector2(0, 1));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(2);
            Destroy(this.gameObject);
        }
    }

}
