using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotDrone : MonoBehaviour
{
    private GameObject player;
    private Vector3 target;
    public GameObject head;
    public Transform shootPoint;
    public Transform spawnPoint;
    public DroneSpawner droneSpawner;

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

    [Header("Other")]
    private bool canSeePlayer;
    public bool engaguePlayer;
    public bool alive = true;

    public GameObject destroyFx;
    public ParticleSystem eyeGlow;

    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.position;
        currentProjectile = projectile;

        StartCoroutine("RandomMovement");
    }

    void Update()
    {
        if (alive)
        {
            if (engaguePlayer)
            {
                AimAtPlayer();
                ShootPlayer();
                CheckForSwitchProjectile();
                Movement();
            }
            else
                FlyBackToSpawn();
        }
    }

    IEnumerator RandomMovement()
    {
        yield return new WaitForSeconds(1);

        float rand = 1;
        float randx = Random.Range(-rand, rand);
        float randz = Random.Range(-rand, rand);

        target = player.transform.position;
        target = new Vector3(target.x + randx, target.y, target.z + randz);

        StartCoroutine("RandomMovement");
    }

    void FlyBackToSpawn()
    {
        movementSpeed = baseMovementSpeed / 4;
        transform.position = Vector3.Lerp(transform.position, spawnPoint.position, movementSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, spawnPoint.transform.position) < 5)
        {
            droneSpawner.RemoveDrone(gameObject, 0);
        }
    }

    void Movement()
    {
        if (Vector3.Distance(transform.position, target) > speedDistanceFromPlayer)
            movementSpeed = baseMovementSpeed / 2;
        else
            movementSpeed = baseMovementSpeed;

        //Debug.Log("Distance to player = " + Vector3.Distance(transform.position, player.transform.position));

        if (Vector3.Distance(transform.position, target) > distanceFromPlayer)
            TravelToPlayer();

        Vector3 playerPos = new Vector3(transform.position.x, target.y + heightDistance, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, playerPos, movementSpeed * Time.deltaTime);

        if (audioSource && audioSource.volume != PlayerPrefs.GetFloat("audioVolume"))
            audioSource.volume = PlayerPrefs.GetFloat("audioVolume");
    }

    void TravelToPlayer()
    {
        Vector3 playerPos = target;
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

    private void OnDestroy()
    {
        if(!alive && destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, Quaternion.identity);

            Destroy(fx, 5);
        }
    }
}
