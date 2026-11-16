
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftFlipper : MonoBehaviour
{
    HingeJoint hinge;
    InputAction leftButt;

    public float flipSpeed = 900f;
    public float flipPower = 1000000f;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        leftButt = InputSystem.actions.FindAction("LeftF");

        JointLimits limits = hinge.limits;
        limits.min = -50f;
        limits.max = 0f;

        hinge.limits = limits;
        hinge.useLimits = true;
        hinge.useMotor = true;
        hinge.useSpring = false;
    }

    void FixedUpdate()
    {
        JointMotor motor = hinge.motor;

        motor.force = flipPower;
        motor.freeSpin = false;

        if (leftButt.IsPressed())
        {
            motor.targetVelocity = -flipSpeed;
        }
        else
        {
            motor.targetVelocity = flipSpeed;
        }

        hinge.motor = motor;
    }
}
