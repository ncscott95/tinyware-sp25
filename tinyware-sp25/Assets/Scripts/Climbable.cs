using UnityEngine;

public class Climbable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerControls>() != null) PlayerControls.Instance.CanClimb = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<PlayerControls>() != null) PlayerControls.Instance.CanClimb = false;
    }
}
