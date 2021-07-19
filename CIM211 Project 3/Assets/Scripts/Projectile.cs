using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;

    public GameObject destroyFx;

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //if(collision.gameObject.GetComponent<Health>())

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, Quaternion.identity);
            Destroy(fx, 2);
        }
    }
}
