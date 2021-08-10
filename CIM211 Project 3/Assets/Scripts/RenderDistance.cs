using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDistance : MonoBehaviour
{
    public string settings = "High";
    public float baseRenderDistance;
    private float renderDistance;

    public GameObject[] objects;
    private Light selfLight;
    private MeshRenderer meshRenderer;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SetSettings();

        player = GameObject.Find("Player");

        if (gameObject.GetComponent<Light>())
            selfLight = gameObject.GetComponent<Light>();

        if (gameObject.GetComponent<MeshRenderer>())
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    void SetSettings()
    {
        // settings = PlayerPrefs.GetString("settings");

        switch (settings)
        {
            case "Low":
                renderDistance = baseRenderDistance / 3;
                break;
            case "Medium":
                renderDistance = (2 * baseRenderDistance) / 3;
                break;
            case "High":
                renderDistance = baseRenderDistance;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDistanceCheck();
    }

    void PlayerDistanceCheck()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < renderDistance)
        {
            ObjectSetActive(true);
        }
        else
        {
            ObjectSetActive(false);
        }
    }

    void ObjectSetActive(bool isActive)
    {
        if (objects.Length > 0)
        {
            foreach (GameObject o in objects)
            {
                o.SetActive(isActive);
            }
        }

        if (selfLight)
        {
            selfLight.enabled = isActive;
        }

        if (meshRenderer)
            meshRenderer.enabled = isActive;
    }
}
