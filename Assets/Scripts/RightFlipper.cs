
using UnityEngine;
using UnityEngine.InputSystem;

public class RightFlipper : MonoBehaviour
{
    HingeJoint hinge;
    InputAction rightButt;

    public float flipSpeed = 900f;
    public float flipPower = 1000000f;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        rightButt = InputSystem.actions.FindAction("RightF");

        JointLimits limits = hinge.limits;
        limits.min = 0f;
        limits.max = 50f;

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

        if (rightButt.IsPressed())
        {
            motor.targetVelocity = flipSpeed;
        }
        else
        {
            motor.targetVelocity = -flipSpeed;
        }

        hinge.motor = motor;
    }
}
