using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public float hitScanDis = 1f;

    public bool isGrenade;
    public float explosionRadius = 1;
    public float explosionForce = 100;

    public GameObject destroyFx;
    public GameObject bulletDecal;
    private Vector3 lastPoint;

    private bool deltDamage;
    private bool hasSpawnedDecal;

    public AudioClip[] destroySounds;

    // Update is called once per frame
    void Update()
    {
        MoveForward();

        if(!isGrenade)
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
        Physics.Raycast(lastPoint, transform.forward, out hit, hitScanDis, ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Vision")));

        if (hit.collider)
        {
            Destroy(gameObject);
        }

        RaycastHit sphereHit;
        Physics.SphereCast(transform.position, 0.1f, transform.forward, out sphereHit, .1f, ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Vision")));

        if (sphereHit.collider && !sphereHit.collider.GetComponent<EnemyHealth>())
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

        SpawnDecal(enemy.transform);

        Destroy(gameObject);
    }

    void SpawnDecal(Transform point)
    {
        if (bulletDecal && !hasSpawnedDecal)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, hitScanDis, ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Vision")));

            if(hit.collider != null)
            {
                //Quaternion dir = new Quaternion(hit.normal.x + 90, hit.normal.y, hit.normal.z, hit.collider.transform.rotation.w);
                GameObject decal = Instantiate(bulletDecal, hit.point, transform.rotation);
                decal.transform.parent = point;
            }
            else
            {
                GameObject decal = Instantiate(bulletDecal, transform.position, transform.rotation);
                decal.transform.parent = point;
            }
           
            //Destroy(decal, 100);

            hasSpawnedDecal = true;
        }
    }

    void DealDamage(PlayerHealth player)
    {
        if (!deltDamage)
        {
            deltDamage = true;
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void MoveForward()
    {
        if(speed > 0)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, hitScanDis, ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Vision")));

            //Debug.DrawRay();

            if (hit.collider)
            {
                if (hit.collider.GetComponent<EnemyHealth>())
                    DealDamage(hit.collider.GetComponent<EnemyHealth>());
                else if(hit.collider.GetComponent<PlayerHealth>())
                    DealDamage(hit.collider.GetComponent<PlayerHealth>());
                else
                {
                    SpawnDecal(hit.collider.transform);
                    Destroy(gameObject);
                }
            }
            else
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<EnemyHealth>())
            DealDamage(collision.gameObject.GetComponent<EnemyHealth>());

        if (collision.gameObject.GetComponent<PlayerHealth>())
        {
            Debug.Log("Hit Player");

            DealDamage(collision.gameObject.GetComponent<PlayerHealth>());
        }

        if (!isGrenade)
            Destroy(gameObject);
        else
            Destroy(gameObject, 0.7f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerHealth>())
        {
            Debug.Log("Hit Player");

            DealDamage(other.gameObject.GetComponent<PlayerHealth>());
        }
    }

    private void OnDestroy()
    {
        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, Quaternion.identity);
            Destroy(fx, 2);
        }

        //if(bulletDecal && !hasSpawnedDecal)
        //{
        //    GameObject decal = Instantiate(bulletDecal, transform.position, transform.rotation);

        //    Destroy(decal, 100);
        //}

        if(destroySounds.Length > 0)
        {
            int rand = Random.Range(0, destroySounds.Length);

            PlaySound(destroySounds[rand]);
        }

        if (isGrenade)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach(Collider ob in colliders)
            {
                Rigidbody rb = ob.GetComponent<Rigidbody>();
                EnemyHealth e = ob.GetComponent<EnemyHealth>();
                PlayerHealth p = ob.GetComponent<PlayerHealth>();

                if(rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                if(e != null)
                {
                    e.TakeDamage(damage);
                }

                if(p != null)
                {
                    p.TakeDamage(damage);
                }
            }
        }
    }

    public void PlaySound(AudioClip sound)
    {
        GameObject soundOb = Instantiate(new GameObject(), transform.position, transform.rotation);
        AudioSource aSource = soundOb.AddComponent<AudioSource>();

        aSource.volume = PlayerPrefs.GetFloat("audioVolume");
        aSource.spatialBlend = 1;
        aSource.minDistance = 5;
        aSource.maxDistance = 200;
        aSource.clip = sound;
        aSource.Play();

        Destroy(soundOb, sound.length);
    }
}
