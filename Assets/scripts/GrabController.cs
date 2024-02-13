using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class grabcontroller : MonoBehaviour
{
    public GameObject otherHand;
    public InputActionReference grabAction;
    public InputActionReference doubleRotationAction;


    private GameObject grabTarget;

    private bool grabTargetHasRigidbody;

    private bool grabbing;
    private Vector3 prevWorldPos;
    private Quaternion prevWorldRot;

    private Vector3 releaseVelocity;
    private Vector3 grabTargetPrevPos;

    private bool toggleDoubleRotation;

    GameObject getGrabTarget()
    {
        return grabTarget;
    }


    void beginGrab(GameObject target)
    {
        grabTarget = target;
        grabTargetHasRigidbody = (target.GetComponent<Rigidbody>() != null);
        grabTargetPrevPos = grabTarget.transform.position;
        grabbing = true;
    }

    void endGrab()
    {
        if (grabTargetHasRigidbody) {
            Rigidbody rb = grabTarget.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = releaseVelocity;
        }
        grabTarget = null;
        grabbing = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        grabbing = false;
        grabTarget = null;
        toggleDoubleRotation = false;

        grabAction.action.Enable();
        doubleRotationAction.action.Enable();

        prevWorldPos = transform.position;
        prevWorldRot = transform.rotation;
    }

    void updateGrabTarget(Vector3 deltaPos, Quaternion deltaRot)
    {
        if (otherHand.GetComponent<grabcontroller>().getGrabTarget() == grabTarget) {
            deltaPos = deltaPos * 0.5f;
            deltaRot = Quaternion.Slerp(Quaternion.identity, deltaRot, 0.5f);
        }

        Vector3 vec = grabTarget.transform.position - transform.position;
        Vector3 rotationCorrection = (deltaRot*vec) - vec;

        grabTarget.transform.root.position = grabTarget.transform.root.position + deltaPos + rotationCorrection;
        grabTarget.transform.root.rotation = deltaRot * grabTarget.transform.root.rotation;


        if (toggleDoubleRotation) {
            grabTarget.transform.root.rotation = deltaRot * grabTarget.transform.root.rotation;
        }



        if (grabTargetHasRigidbody) {
            grabTarget.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void updateReleaseVelocities()
    {
        Vector3 deltaPos = grabTarget.transform.position - grabTargetPrevPos;
        releaseVelocity = (releaseVelocity*0.75f) + ((deltaPos / Time.deltaTime)*0.25f);
        grabTargetPrevPos = grabTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPos = transform.position - prevWorldPos;
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(prevWorldRot);

        prevWorldPos = transform.position;
        prevWorldRot = transform.rotation;

        if (grabbing) {
            updateGrabTarget(deltaPos, deltaRot);
            updateReleaseVelocities();
        }

        toggleDoubleRotation = (doubleRotationAction.action.triggered ? !toggleDoubleRotation : toggleDoubleRotation);

        float grabVal = grabAction.action.ReadValue<float>();
        float grabBeginTreshold = 0.75f;
        float grabEndTreshold = 0.25f;

        if (grabVal > grabBeginTreshold && !grabbing && grabTarget != null) {
            beginGrab(grabTarget);
        } else if (grabVal < grabEndTreshold && grabbing) {
            endGrab();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!grabbing) {
            if (other.transform.root != transform.root) {
                grabTarget = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!grabbing) {
            if (other.gameObject == grabTarget) {
                grabTarget = null;
            }
        }
    }
}
