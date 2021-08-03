using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPlatform : MonoBehaviour
{
    public float timer;
    public float lifetime;
    
    void Start()
    {
        timer = 0f;
        lifetime = 3f;
    }

    
    void Update()
    {
        if(timer < lifetime)
        {
            timer += Time.deltaTime;
        }
        else {
            this.gameObject.SetActive(false);
            timer = lifetime;
        }
    }
}
