using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    public GameObject dronePrefab;
    public GameObject player;

    public Transform[] spawnPoints;

    public float spawnHeight;

    public int maxDroneCount = 15;
    public float droneSpawnIntival = 2;
    private float droneSpawnIntivalTimer;

    public List<GameObject> spawnedDrones;

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
    }

    void SpawnDrone()
    {
        droneSpawnIntivalTimer = droneSpawnIntival;

        int rand = Random.Range(0, spawnPoints.Length);
        GameObject drone = Instantiate(dronePrefab, spawnPoints[rand].position, spawnPoints[rand].rotation);

        drone.GetComponent<RiotDrone>().spawnPoint = spawnPoints[rand];
        drone.GetComponent<RiotDrone>().droneSpawner = this;

        spawnedDrones.Add(drone);
    }

    public void RemoveDrone(GameObject drone)
    {
        spawnedDrones.Remove(drone);
        Destroy(drone);
    }

    bool PlayerHeight()
    {
        if(player.transform.position.y > spawnHeight)
            return true;
        else
            return false;
    }
}
