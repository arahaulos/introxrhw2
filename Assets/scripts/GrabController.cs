using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class grabcontroller : MonoBehaviour
{
    public InputActionReference grabAction;

    private GameObject grabTarget;
    private bool grabbing;

    // Start is called before the first frame update
    void Start()
    {
        grabAction.action.Enable();
        grabTarget = null;
        grabbing = false;
    }

    void beginGrab()
    {
        grabTarget.AddComponent<FixedJoint>();  
		grabTarget.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();

        grabbing = true;
    }

    void endGrab()
    {
        Destroy(grabTarget.GetComponent<FixedJoint>());
        grabbing = false;
        grabTarget = null;
    }
    

    // Update is called once per frame
    void Update()
    {
        float grabValue = grabAction.action.ReadValue<float>();
        if (grabValue > 0.75f && !grabbing && grabTarget != null) {
            beginGrab();
        } else if (grabValue < 0.25f && grabbing && grabTarget != null) {
            endGrab();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!grabbing && other.gameObject.tag == "Grabbable" && other.gameObject.GetComponent<Rigidbody>() != null) {
            grabTarget = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!grabbing && other.gameObject == grabTarget) {
            grabTarget = null;
        }
    }
}