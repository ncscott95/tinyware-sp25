using UnityEngine;

public class MomoTheCat : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        Invoke("WalkAway", 2f);
    }

    void WalkAway()
    {
        animator.SetTrigger("Walk");
        transform.LeanMoveLocalX(transform.position.x + 10f, 3f);
    }
}
