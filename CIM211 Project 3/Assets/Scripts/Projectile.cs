using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public float hitScanDis = 0.5f;

    public bool isGrenade;
    public float explosionRadius = 1;
    public float explosionForce = 100;

    public GameObject destroyFx;
    private Vector3 lastPoint;

    private bool deltDamage;

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        CheckForCollision();
    }

    private void FixedUpdate()
    {
        //CheckForCollision();
    }

    void CheckForCollision()
    {
        //RaycastHit sphereHit;
        //Physics.SphereCast(transform.position, 0.1f, Vector3.forward, out sphereHit);

        //if (sphereHit.collider)
        //{
        //    if (sphereHit.collider.GetComponent<EnemyHealth>())
        //    {
        //        //DealDamage(sphereHit.collider.GetComponent<EnemyHealth>());
        //    }
        //    else
        //    {
        //        if (lastPoint != null)
        //        {
        //            RaycastHit hit;
        //            Physics.Raycast(lastPoint, transform.position, out hit, hitScanDis, LayerMask.GetMask("PlayerProjectile"));

        //            if (hit.collider)
        //            {
        //                Destroy(gameObject);
        //            }
        //        }
        //        else
        //        {
        //            lastPoint = transform.position;
        //        }
        //    }
        //}

        RaycastHit hit;
        Physics.Raycast(lastPoint, transform.position, out hit, hitScanDis, LayerMask.GetMask("PlayerProjectile"));

        if (hit.collider && hit.collider.GetComponent<EnemyHealth>() == null)
        {
            Destroy(gameObject);
        }
    }

    void DealDamage(EnemyHealth enemy)
    {
        if (!deltDamage)
        {
            deltDamage = true;
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void MoveForward()
    {
        if(speed > 0)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<EnemyHealth>())
        {
            DealDamage(collision.gameObject.GetComponent<EnemyHealth>());
        }

        if (!isGrenade)
            Destroy(gameObject);
        else
            Destroy(gameObject, 0.7f);
    }

    private void OnDestroy()
    {
        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, Quaternion.identity);
            Destroy(fx, 2);
        }

        if (isGrenade)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach(Collider ob in colliders)
            {
                Rigidbody rb = ob.GetComponent<Rigidbody>();
                EnemyHealth e = ob.GetComponent<EnemyHealth>();

                if(rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                if(e != null)
                {
                    e.TakeDamage(damage);
                }
            }
        }
    }
}
