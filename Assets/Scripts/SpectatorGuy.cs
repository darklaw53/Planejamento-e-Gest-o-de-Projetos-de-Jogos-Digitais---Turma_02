using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorGuy : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        StartCoroutine(InfiniteLoop());
    }

    private IEnumerator InfiniteLoop()
    {
        WaitForSeconds waitTime = new WaitForSeconds(Random.Range(1, 5));
        while (true)
        {
            anim.Play("Cheer");
            yield return waitTime;
        }
    }
}
