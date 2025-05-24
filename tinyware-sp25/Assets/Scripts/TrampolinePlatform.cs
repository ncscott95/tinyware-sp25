using UnityEngine;

public class TrampolinePlatform : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        rb.linearVelocityY *= -1f;
    }
}
