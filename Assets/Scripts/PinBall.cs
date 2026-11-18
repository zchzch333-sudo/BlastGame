using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

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

    public static float blastValue = 0f;
    public float blastValueMax = 100f;

    public Image blastValueBar;

    public static bool blastMode = false;
    public GameObject blastModeBlock;

    public float blastTime = 6f;
    static float blastEndTime;

    public float blastSpeed = 20f;
    public GameObject gameOverUI;

    void Start()
    {
        ballLeft = 20;

        rigidBody = GetComponent<Rigidbody>();
        rollButt = InputSystem.actions.FindAction("Roll");
        startPosition = transform.position;

        scoreText.text = "Score: 0";
        ballLeftText.text = ballLeft.ToString("D2");

        blastValue = 0f;
        blastMode = false;
        blastModeBlock.SetActive(false);
        gameOverUI.SetActive(false);
        blastValueBar.fillAmount = 0f;
        float blastEndTime = 0f;
    }


    void Update()
    {
        if (blastValue >= blastValueMax && blastMode == false)
        {
            BlastMode();
        }

        if (blastMode == true)
        {
            blastValue = Mathf.Max(0f, (blastEndTime - Time.time) / blastTime * blastValueMax);

            if (Time.time >= blastEndTime)
            {
                blastMode = false;
                blastValue = 0f;
                blastModeBlock.SetActive(false);
            }
        }

        blastValueBar.fillAmount = blastValue / blastValueMax;
        blastModeBlock.SetActive(blastMode);


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

        if (blastMode == true && isRolling == true)
        {
            if (rigidBody.linearVelocity.magnitude < blastSpeed &&
                rigidBody.linearVelocity.magnitude > 0.1f)
            {
                rigidBody.linearVelocity = rigidBody.linearVelocity.normalized * blastSpeed;
            }
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



    private void OnCollisionEnter(Collision collision)
    {
        if (blastMode) return;

        if (collision.gameObject.CompareTag("Bumper"))
        {
            blastValue += 5f;
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            blastValue += 10f;
        }

        blastValue = Mathf.Clamp(blastValue, 0f, blastValueMax);
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
    void BlastMode()
    {
        blastMode = true;
        blastEndTime = Time.time + blastTime;
        blastModeBlock.SetActive(true);
    }
    void EndGame()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;

    }
}
