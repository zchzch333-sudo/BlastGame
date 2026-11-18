
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    public float bouncePower = 15f;
    public float bossLife = 10;
    float maxBossLife;

    public Image bossHealthBar;
    public GameObject winUI;


    void Start()
    {
        maxBossLife = bossLife;
        bossHealthBar.fillAmount = 1f;
        winUI.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ball = collision.rigidbody;

            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;

            ball.AddForce(direction * bouncePower, ForceMode.Impulse);
            if (bossLife > 0)
            {
                bossLife -= 1;
                bossHealthBar.fillAmount = bossLife / maxBossLife;

                if (bossLife == 0)
                {
                    WinGame();
                }
            }
        }
    }
    void WinGame()
    {
        winUI.SetActive(true);
        Time.timeScale = 0f;

    }
}
