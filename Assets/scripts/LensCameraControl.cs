using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensCameraControl : MonoBehaviour
{
    public Transform lensCameraTransform;
    public Transform mainCameraTransform; 
    public Transform magGlassTransform;
    public Camera lensCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(lensCameraTransform.position, mainCameraTransform.position);


        lensCameraTransform.rotation = mainCameraTransform.rotation;

        lensCameraTransform.LookAt(mainCameraTransform.position, magGlassTransform.rotation * Vector3.up);
        lensCameraTransform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
