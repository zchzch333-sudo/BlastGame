
using UnityEngine;
public class Bumper : MonoBehaviour
{
    public float bouncePower = 15f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ball = collision.rigidbody;

            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;

            ball.AddForce(direction * bouncePower, ForceMode.Impulse);
        }
    }
}
