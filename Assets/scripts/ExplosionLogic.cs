using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLogic : MonoBehaviour
{
    private float destructionTime = -1.0f;


    public void Play()
    {
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();

        destructionTime = Time.time + 3.0f;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destructionTime > 0.0f && destructionTime > Time.time) {
            //Destroy(gameObject);
        }
    }
}
