using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleExpand : MonoBehaviour
{
    private float time = 3;
    Vector3 startSize;

    void Start()
    {
        startSize = transform.localScale;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        //StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (transform.localScale.x > startSize.x*10) Destroy(gameObject);
        else transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 2, Time.unscaledDeltaTime * 5);
    }
}