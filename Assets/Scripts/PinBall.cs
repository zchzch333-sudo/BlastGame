using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PinBall : MonoBehaviour
{
    Rigidbody rigidBody;

    InputAction rollButt;

    bool holdingButt;
    bool roll;
    public TMP_Text scoreText;
    public TMP_Text ballLeftText;
    public static int ballLeft = 20;
    Vector3[] startPositions;
    Quaternion[] startRotations;
    int score;
    Vector3 startPosition;

    public float rollPower = 30f;
    public float holdTime = 0;
    public bool isRolling = false;
    public bool canLaunch = false;

    void Start()
    {
        ballLeft = 20;

        rigidBody = GetComponent<Rigidbody>();
        rollButt = InputSystem.actions.FindAction("Roll");
        startPosition = transform.position;

        scoreText.text = "Score: 0";
        ballLeftText.text = ballLeft.ToString("D2");
    }


    void Update()
    {
        if (canLaunch == false)
        {
            return;
        }

        if (rollButt.WasPressedThisFrame())
        {
            holdingButt = true;
        }
        else if (rollButt.WasReleasedThisFrame())
        {
            holdingButt = false;
            roll = true;
        }

        if (holdingButt)
        {
            holdTime += Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        if (roll)
        {
            StartRoll();
        }

        if (isRolling)
        {
            rigidBody.AddForce(Vector3.down * 40f, ForceMode.Acceleration);
        }
    }
    void StartRoll()
    {
        Vector3 pointTo = new Vector3(0, 0, 1);

        rigidBody.AddForce(Mathf.Clamp(rollPower + holdTime * 30f, rollPower * 3f, rollPower * 5f) * pointTo, ForceMode.Impulse);
        roll = false;
        isRolling = true;
        holdTime = 0f;

        canLaunch = false;
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reset") && isRolling)
        {
            ballLeft -= 1;
            ballLeftText.text = Mathf.Max(0, ballLeft).ToString("D2");

            if (ballLeft >= 0)
            {
                ResetBall();
            }
            else
            {
                if (ballLeft == -2)
                {
                    EndGame();
                }

                gameObject.SetActive(false);
            }
        }
        if (other.CompareTag("LaunchZone"))
        {
            canLaunch = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LaunchZone"))
        {
            canLaunch = false;
            holdingButt = false;
            holdTime = 0f;
        }
    }


    void ResetBall()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        isRolling = false;
        canLaunch = true;

        scoreText.text = "Score: " + score;
    }
    void EndGame()
    {

    }
}
