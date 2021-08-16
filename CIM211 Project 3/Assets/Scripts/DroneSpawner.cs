using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    public GameObject dronePrefab;
    public GameObject player;

    public Transform[] spawnPoints;
    public bool randomSpawnPoint;

    public float spawnHeight;

    public int baseMaxDroneCount = 15;
    public int maxDroneCount;
    public float baseDroneSpawnIntival = 2;
    public float droneSpawnIntival;
    private float droneSpawnIntivalTimer;

    public List<GameObject> spawnedDrones;

    private float timer;

    void Start()
    {
        spawnedDrones = new List<GameObject>();
    }

    void Update()
    {
        if (PlayerHeight())
            SpawnDrones();
        else
            DronesFlyAway();
    }

    void DronesFlyAway()
    {
        if(spawnedDrones.Count > 0)
        {
            foreach(GameObject drone in spawnedDrones)
            {
                RiotDrone rt = drone.GetComponent<RiotDrone>();
                rt.engaguePlayer = false;
            }
        }

        timer = 0;
    }

    void SpawnDrones()
    {
        if (droneSpawnIntivalTimer <= 0)
        {
            if(spawnedDrones.Count < maxDroneCount)
                SpawnDrone();
        }
        else
            droneSpawnIntivalTimer -= Time.deltaTime;

        if (spawnedDrones.Count > 0)
        {
            foreach (GameObject drone in spawnedDrones)
            {
                RiotDrone rt = drone.GetComponent<RiotDrone>();
                rt.engaguePlayer = true;
            }
        }

        timer += Time.deltaTime;
        IncreaseFrequency();
    }

    void IncreaseFrequency()
    {
        if(timer > 90)
        {
            maxDroneCount = baseMaxDroneCount * 3;
            droneSpawnIntival = baseDroneSpawnIntival - ((baseDroneSpawnIntival * 2) / 3);
        }
        else if(timer > 60)
        {
            maxDroneCount = baseMaxDroneCount * 2;
            droneSpawnIntival = baseDroneSpawnIntival - (baseDroneSpawnIntival / 2);
        }
        else if(timer > 30)
        {
            maxDroneCount = baseMaxDroneCount + 5;
            droneSpawnIntival = baseDroneSpawnIntival - (baseDroneSpawnIntival / 4);
        }
        else
        {
            maxDroneCount = baseMaxDroneCount;
            droneSpawnIntival = baseDroneSpawnIntival;
        }
    }

    void SpawnDrone()
    {
        droneSpawnIntivalTimer = droneSpawnIntival;

        Vector3 point = new Vector3();

        if (!randomSpawnPoint)
        {
            int rand = Random.Range(0, spawnPoints.Length);
            point = spawnPoints[rand].position;
        }
        else
        {
            int value = 100;
            int randx = Random.Range(100, 400);
            int randz = Random.Range(-50, 300);

            point = new Vector3(randx, 100, randz);
        }

        GameObject drone = Instantiate(dronePrefab, point, spawnPoints[0].rotation);

        drone.GetComponent<RiotDrone>().spawnPoint = point;
        drone.GetComponent<RiotDrone>().droneSpawner = this;

        spawnedDrones.Add(drone);
    }

    public void RemoveDrone(GameObject drone, float time)
    {
        spawnedDrones.Remove(drone);
        Destroy(drone, time);
    }

    bool PlayerHeight()
    {
        if(player.transform.position.y > spawnHeight)
            return true;
        else
            return false;
    }
}
