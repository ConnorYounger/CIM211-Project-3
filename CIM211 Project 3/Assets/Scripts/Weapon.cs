using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool right;
    public Transform playerCam;

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
    public float baseBulletSpread = 0.05f;
    private float bulletSpread;

    [Header("Melee Weapon")]
    public MeleeCollision meleeCollision;
    public float hitDelayTime = 0.1f;
    public float hitFinishTime = 0.3f;
    public int multiHitCount = 1;

    [Header("Animation")]
    public Animator animator;
    public string fireAnimation = "Fire";

    private bool canFire = true;
    public bool canUse = true;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] fireSounds;

    void Start()
    {
        if (meleeCollision)
            meleeCollision.SetStats(damage, multiHitCount, hitDelayTime, hitFinishTime);

        //bulletSpread = baseBulletSpread;

        if (gameObject.GetComponent<AudioSource>())
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("audioVolume");
        }
    }

    void Update()
    {
        if (canFire && canUse)
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
            animator.Play(fireAnimation);
        }

        if (audioSource)
        {
            if(fireSounds.Length == 0)
                audioSource.Play();
            else
            {
                int rand = Random.Range(0, fireSounds.Length);

                audioSource.clip = fireSounds[rand];
                audioSource.Play();
            }
        }
    }

    void FireRateCoolDown()
    {
        if (canUse)
        {
            if (fireRateCoolDown > 0 && !canFire)
            {
                fireRateCoolDown -= Time.deltaTime;
            }
            else if (!canFire)
            {
                canFire = true;
            }
        }
    }

    void FireProjectile()
    {
        if (projectile && playerCam)
        {
            RaycastHit hit;
            Vector3 shootDir = playerCam.forward;
            shootDir.x += Random.Range(-bulletSpread, bulletSpread);
            shootDir.y += Random.Range(-bulletSpread, bulletSpread);

            Physics.Raycast(playerCam.position, shootDir, out hit, 1000, ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Vision")));

            GameObject proj = Instantiate(projectile, shootPoint.position, shootPoint.rotation);

            if(hit.collider != null)
                proj.transform.LookAt(hit.point);

            proj.GetComponent<Projectile>().damage = damage;
            proj.GetComponent<Projectile>().speed = projectileSpeed;
            Destroy(proj, 5);

            if(launchForce > 0)
            {
                proj.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * launchForce);
            }
        }
        else
            Debug.LogError("No Projectile/PlayerCam Was Set");
    }

    public void SetBloom(float bloomMultiplier)
    {
        if (bloomMultiplier > 0)
        {
            float i = baseBulletSpread / bloomMultiplier;
            bulletSpread = i;

            //Debug.Log("set " + gameObject.name + " bloom to " + bulletSpread);
        }
    }
}
