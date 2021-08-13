using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotDrone : MonoBehaviour
{
    private GameObject player;
    public GameObject head;
    public Transform shootPoint;

    [Header("Projectile")]
    public GameObject projectile;
    public GameObject missile;
    public float fireRate = 2;
    private float fireRatetimer;
    public float projectileSwitchTime = 5;
    private float projectileSwitchTimer;
    private GameObject currentProjectile;

    [Header("Movement")]
    public float baseMovementSpeed = 10;
    private float movementSpeed;
    public float distanceFromPlayer = 4;
    public float speedDistanceFromPlayer = 20;
    public float heightDistance = 3;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource shootAudioSource;

    private bool canSeePlayer;

    void Start()
    {
        player = GameObject.Find("Player");
        currentProjectile = projectile;
    }

    void Update()
    {
        AimAtPlayer();
        ShootPlayer();
        CheckForSwitchProjectile();
        Movement();
    }

    void Movement()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > speedDistanceFromPlayer)
            movementSpeed = baseMovementSpeed * 3;
        else
            movementSpeed = baseMovementSpeed;

        //Debug.Log("Distance to player = " + Vector3.Distance(transform.position, player.transform.position));

        if (Vector3.Distance(transform.position, player.transform.position) > distanceFromPlayer)
            TravelToPlayer();

        Vector3 playerPos = new Vector3(transform.position.x, player.transform.position.y + heightDistance, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, playerPos, movementSpeed * Time.deltaTime);

        if (audioSource && audioSource.volume != PlayerPrefs.GetFloat("audioVolume"))
            audioSource.volume = PlayerPrefs.GetFloat("audioVolume");
    }

    void TravelToPlayer()
    {
        Vector3 playerPos = player.transform.position;
        playerPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);

        transform.position = Vector3.Lerp(transform.position, playerPos, movementSpeed * Time.deltaTime);
    }

    void CheckForSwitchProjectile()
    {
        if (!canSeePlayer)
        {
            if (projectileSwitchTimer < projectileSwitchTime)
                projectileSwitchTimer += Time.deltaTime;
            else
                SwitchProjectile(missile);
        }
        else
        {
            projectileSwitchTimer = 0;
            SwitchProjectile(projectile);
        }
    }

    void SwitchProjectile(GameObject projectile)
    {
        if(currentProjectile != projectile)
        {
            currentProjectile = projectile;
        }
    }

    void ShootPlayer()
    {
        if (canSeePlayer || currentProjectile == missile)
        {
            if (fireRatetimer <= 0)
                FireProjectile();
            else
                fireRatetimer -= Time.deltaTime;
        }
    }

    void FireProjectile()
    {
        fireRatetimer = fireRate;

        GameObject p = Instantiate(currentProjectile, shootPoint.position, shootPoint.rotation);

        p.layer = 11;
        p.GetComponent<Projectile>().speed = 30;
        p.GetComponent<Projectile>().damage = 20;

        Destroy(p, 10);

        if (audioSource)
        {
            shootAudioSource.volume = PlayerPrefs.GetFloat("audioVolume");
            shootAudioSource.Play();
        }
    }

    RaycastHit CalculateHit(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        Vector3 rayAngle = (end - start).normalized;

        Physics.Raycast(start, rayAngle, out hit);
        return hit;
    }

    RaycastHit CalculateHit(Vector3 start, Vector3 end, int layerMask)
    {
        RaycastHit hit;
        Vector3 rayAngle = (end - start).normalized;

        Physics.Raycast(start, rayAngle, out hit, 100, layerMask);
        return hit;
    }

    void AimAtPlayer()
    {
        head.transform.LookAt(player.transform.position);

        RaycastHit hit = CalculateHit(shootPoint.position, player.transform.position, ~LayerMask.GetMask("Vision"));

        if (hit.collider && hit.collider.gameObject.name == "Player")
            canSeePlayer = true;
        else
            canSeePlayer = false;
    }
}
