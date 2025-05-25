using UnityEngine;

public class DeathField : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerControls>() != null) PlayerControls.Instance.Death();
    }
}
