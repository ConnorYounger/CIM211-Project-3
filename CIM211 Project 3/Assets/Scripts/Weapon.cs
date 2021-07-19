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

    [Header("Animation")]
    public Animator animator;

    private bool canFire = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (canFire)
        {
            if (!right && Input.GetButton("Fire1"))
                FireWeapon();
            else if (right && Input.GetButton("Fire2"))
                FireProjectile();
        }
        

        FireRateCoolDown();
    }

    void FireWeapon()
    {
        // Shoot Weapon
        if (isProjectileWeapon)
            FireProjectile();

        if(animator)
            animator.Play("");

        fireRateCoolDown = fireRate + fireAnimationBuffer;
        canFire = false;
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
        }
        else
            Debug.LogError("No Projectile Was Set");
    }
}
