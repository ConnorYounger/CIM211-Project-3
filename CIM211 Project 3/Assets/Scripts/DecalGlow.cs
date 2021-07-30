using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalGlow : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public float fadeOutDelayTime = 2;
    public float fadeOutMultiplier = 1;

    private float fadeOutValue = 100;

    private bool fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<MeshRenderer>())
            meshRenderer = gameObject.GetComponent<MeshRenderer>();

        StartCoroutine("StartFadeOut");
    }

    IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(fadeOutDelayTime);

        fadeOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
            FadeOut();
    }

    void FadeOut()
    {
        if(meshRenderer.material.color.a > 0)
        {
            float a = meshRenderer.material.color.a - (fadeOutMultiplier * Time.deltaTime);
            meshRenderer.material.color = new Color(meshRenderer.material.color.r, meshRenderer.material.color.g, meshRenderer.material.color.b, a);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
