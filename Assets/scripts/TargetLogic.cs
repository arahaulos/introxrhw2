using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLogic : MonoBehaviour
{
    public GameObject counterBoard;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Grabbable") {
            counterBoard.GetComponent<TargetCounterBoard>().targetHit(gameObject);

            GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionObject.GetComponent<ExplosionLogic>().Play();

            Destroy(transform.parent.gameObject);
            Destroy(c.gameObject);
        }
    }
}
