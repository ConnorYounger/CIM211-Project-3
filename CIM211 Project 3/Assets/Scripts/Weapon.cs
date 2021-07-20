using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool right;

    [Header("Stats")]
    public float damage = 10;
    public float fireRate = 1;
    private float fireRateCoolDown;
    public float fireAnimationBuffer = 1;

    [Header("Projectile Weapon")]
    public bool isProjectileWeapon;
    public GameObject projectile;
    public Transform shootPoint;
    public float projectileSpeed = 10;
    public float launchForce = 50;

    [Header("Melee Weapon")]
    public MeleeCollision meleeCollision;
    public float hitDelayTime = 0.1f;
    public float hitFinishTime = 0.3f;
    public int multiHitCount = 1;

    [Header("Animation")]
    public Animator animator;

    private bool canFire = true;

    void Start()
    {
        if (meleeCollision)
            meleeCollision.SetStats(damage, multiHitCount, hitDelayTime, hitFinishTime);
    }

    void Update()
    {
        if (canFire)
        {
            if (!right && Input.GetButton("Fire1"))
                FireWeapon();
            else if (right && Input.GetButton("Fire2"))
                FireWeapon();
        }
        

        FireRateCoolDown();
    }

    void FireWeapon()
    {
        fireRateCoolDown = fireRate + fireAnimationBuffer;
        canFire = false;

        // Shoot Weapon
        if (isProjectileWeapon)
            FireProjectile();
        else
            meleeCollision.StartCoroutine("MeleeAttack");

        if (animator)
        {
            animator.Play("Fire");
        }
    }

    void FireRateCoolDown()
    {
        if(fireRateCoolDown > 0 && !canFire)
        {
            fireRateCoolDown -= Time.deltaTime;
        }
        else if (!canFire)
        {
            canFire = true;
        }
    }

    void FireProjectile()
    {
        if (projectile)
        {
            GameObject proj = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
            proj.GetComponent<Projectile>().damage = damage;
            proj.GetComponent<Projectile>().speed = projectileSpeed;
            Destroy(proj, 5);

            if(launchForce > 0)
            {
                proj.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * launchForce);
            }
        }
        else
            Debug.LogError("No Projectile Was Set");
    }
}
