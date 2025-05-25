using UnityEngine;

public class DeathField : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerControls.Instance.Death();
    }
}
