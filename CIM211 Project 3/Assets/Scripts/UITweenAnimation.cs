using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITweenAnimation : MonoBehaviour
{
    private Vector3 defultScale;
    public float targetScale;
    public float rate = 1;

    private bool dir;

    void Start()
    {
        //transform.localScale = defultScale;
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    void Animate()
    {
        Debug.Log(transform.localScale.magnitude);

        if (transform.localScale.magnitude >= targetScale && !dir)
        {
            dir = true;
        }
        else if (transform.localScale.magnitude <= 1 && dir)
        {
            dir = false;
        }

        if (!dir)
        {
            transform.localScale = Vector3.Scale(transform.localScale, transform.localScale * rate);
        }
        else
        {
            transform.localScale = Vector3.Scale(transform.localScale, transform.localScale / rate);
        }
    }
}
