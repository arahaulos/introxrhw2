using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject grabbableObject;
    public GameObject tragetObject;

    private List<GameObject> grabbableObjects = new List<GameObject>();
    private List<GameObject> targetObjects = new List<GameObject>();

    private int targetSpawnCounter = 0;

    private Vector3 grabbableSpawnMins = new Vector3(0.0f, 0.7f, 0.7f);
    private Vector3 grabbableSpawnMaxs = new Vector3(1.0f, 1.5f, 1.25f);

    private float maxTargetDistance = 4.0f;
    private float minTargetDistance = 2.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    void removeNullReferences() {
        for (int i = 0; i < grabbableObjects.Count; i++) {
            if (grabbableObjects[i] == null) {
                grabbableObjects.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < targetObjects.Count; i++) {
            if (targetObjects[i] == null) {
                targetObjects.RemoveAt(i);
                i--;
            }
        }
    }

    int countGrabbableObjectsOnTable() {
        int count = 0;
        for (int i = 0; i < grabbableObjects.Count; i++) {
            Vector3 pos = grabbableObjects[i].transform.position;
            if (pos.x > grabbableSpawnMins.x && pos.x < grabbableSpawnMaxs.x &&
                pos.y > grabbableSpawnMins.y && pos.y < grabbableSpawnMaxs.y &&
                pos.z > grabbableSpawnMins.z && pos.z < grabbableSpawnMaxs.z) {
                count++;
            }
        }
        return count;
    }

    void spawnGrabbable() {
        Vector3 newPos = new Vector3(Random.Range(grabbableSpawnMins.x+0.1f, grabbableSpawnMaxs.x-0.1f), 
                                     grabbableSpawnMaxs.y-0.1f, 
                                     Random.Range(grabbableSpawnMins.z+0.1f, grabbableSpawnMins.z+0.1f));

        GameObject newObject = Instantiate(grabbableObject, newPos, Quaternion.identity);
        newObject.GetComponent<Rigidbody>().isKinematic = false;

        grabbableObjects.Add(newObject);
    }

    void setObjectLayer(GameObject obj, LayerMask layer) {
        obj.layer = layer;
        foreach (Transform tf in obj.transform) {
            setObjectLayer(tf.gameObject, layer);
        }
    }

    void spawnTarget() {
        float randAngle = Random.Range(-Mathf.PI, Mathf.PI);
        float randDist = Random.Range(minTargetDistance, maxTargetDistance);
        
        Vector3 newPos = new Vector3(Mathf.Sin(randAngle)*randDist, 0.4f, Mathf.Cos(randAngle)*randDist);

        Quaternion lookRot = Quaternion.LookRotation(newPos, Vector3.up);

        GameObject newTarget = Instantiate(tragetObject, newPos, Quaternion.Euler(0.0f, lookRot.eulerAngles.y, 0.0f));
        if ((targetSpawnCounter % 4) == 0) {
            setObjectLayer(newTarget, LayerMask.NameToLayer("OnlyVisibleForLens"));
        }
        targetObjects.Add(newTarget);

        targetSpawnCounter++;
    }

    // Update is called once per frame
    void Update()
    {
        removeNullReferences();

        if (countGrabbableObjectsOnTable() < 3) {
            spawnGrabbable();
        }

        if (targetObjects.Count < 3) {
            spawnTarget();
        }
    }
}
