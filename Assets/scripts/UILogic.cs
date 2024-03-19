using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UILogic : MonoBehaviour
{
    public InputActionReference leftHandPrimaryButton;
    public InputActionReference rightHandPrimaryButton;
    public InputActionReference menuEnterButton;

    public GameObject UIcanvas;

    public GameObject leftHandController;
    public GameObject rightHandController;

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject playButtonTrigger;
    public GameObject exitButtonTrigger;

    public bool uiEnabled;

    // Start is called before the first frame update
    void Start()
    {
        leftHandPrimaryButton.action.Enable();
        rightHandPrimaryButton.action.Enable();
        menuEnterButton.action.Enable();

        enableUI(true);
    }


    void enableHand(GameObject obj, bool enable) {
        Collider[] colliders = obj.GetComponents<Collider>();   
        if (colliders != null) {
            foreach (Collider coll in colliders) {
                coll.enabled = enable;
            }
        }

        SkinnedMeshRenderer renderer = obj.GetComponent<SkinnedMeshRenderer>();
        if (renderer != null) {
            renderer.enabled = enable;
        }

        foreach (Transform tf in obj.transform) {
            enableHand(tf.gameObject, enable);
        }
    }

    public void enableUI(bool enable) {
        playButtonTrigger.SetActive(enable);
        exitButtonTrigger.SetActive(enable);

        leftHandController.GetComponent<XRInteractorLineVisual>().enabled  = enable;
        rightHandController.GetComponent<XRInteractorLineVisual>().enabled  = enable;

        leftHandController.GetComponent<LineRenderer>().enabled  = enable;
        rightHandController.GetComponent<LineRenderer>().enabled  = enable;

        enableHand(leftHand, !enable);
        enableHand(rightHand, !enable);

        UIcanvas.SetActive(enable);
        uiEnabled = enable;
    }

    void playButtonPress()
    {
        enableUI(false);
    }

    void exitButtonPress()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void buttonPress(Transform t) {
        RaycastHit hit;
        if (Physics.Raycast(t.position, t.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.collider.gameObject == playButtonTrigger) {
                playButtonPress();
            } else if (hit.collider.gameObject == exitButtonTrigger) {
                exitButtonPress();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiEnabled) {
            if (menuEnterButton.action.triggered) {
                enableUI(true);
            }
            return;
        }

        if (leftHandPrimaryButton.action.triggered) {
            buttonPress(leftHandController.transform);
        }
        if (rightHandPrimaryButton.action.triggered) {
            buttonPress(rightHandController.transform);
        }

    }
}
