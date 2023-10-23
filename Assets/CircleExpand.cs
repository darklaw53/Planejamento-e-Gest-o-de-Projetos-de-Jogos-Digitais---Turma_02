using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleExpand : MonoBehaviour
{
    private float time = 3;

    void Start()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 2, Time.unscaledDeltaTime * 5);
    }
}