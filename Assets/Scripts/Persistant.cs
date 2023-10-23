using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistant : Singleton<Persistant>
{
    public int challengeRating;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
