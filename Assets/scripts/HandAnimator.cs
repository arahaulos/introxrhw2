using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{

    public InputActionReference grabAction;
    public InputActionReference triggerAction;
    
    public HingeJoint indexFingerJoint0;
    public HingeJoint indexFingerJoint1;
    public HingeJoint indexFingerJoint2;

    public HingeJoint middleFingerJoint0;
    public HingeJoint middleFingerJoint1;
    public HingeJoint middleFingerJoint2;

    public HingeJoint ringFingerJoint0;
    public HingeJoint ringFingerJoint1;
    public HingeJoint ringFingerJoint2;

    public HingeJoint PinkyJoint0;
    public HingeJoint PinkyJoint1;
    public HingeJoint PinkyJoint2;

    // Start is called before the first frame update
    void Start()
    {
        grabAction.action.Enable();
        triggerAction.action.Enable();
    }

    void setStringTarget(HingeJoint hinge, float val) 
    {
        float target = (hinge.limits.min * val) + (hinge.limits.max * (1.0f - val));

        JointSpring hingeSpring = hinge.spring;
        hingeSpring.spring = 50;
        hingeSpring.damper = 2;
        hingeSpring.targetPosition = target;

        hinge.spring = hingeSpring;
        hinge.useSpring = true;
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>() * 0.75f;
        float grabValue = grabAction.action.ReadValue<float>() * 0.75f;

        setStringTarget(indexFingerJoint0, triggerValue);
        setStringTarget(indexFingerJoint1, triggerValue);
        setStringTarget(indexFingerJoint2, triggerValue);

        setStringTarget(middleFingerJoint0, grabValue);
        setStringTarget(middleFingerJoint1, grabValue);
        setStringTarget(middleFingerJoint2, grabValue);

        setStringTarget(ringFingerJoint0, grabValue);
        setStringTarget(ringFingerJoint1, grabValue);
        setStringTarget(ringFingerJoint2, grabValue);

        setStringTarget(PinkyJoint0, grabValue);
        setStringTarget(PinkyJoint1, grabValue);
        setStringTarget(PinkyJoint2, grabValue);
    }
}
