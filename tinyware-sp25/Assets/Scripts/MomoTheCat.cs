using UnityEngine;

public class MomoTheCat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool exitRight = true;

    void Start()
    {
        Invoke("WalkAway", 2f);
    }

    void WalkAway()
    {
        animator.SetTrigger("Walk");
        if (exitRight)
        {
            transform.LeanMoveLocalX(transform.position.x + 10f, 3f);
        }
        else
        {
            transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            transform.LeanMoveLocalX(transform.position.x - 10f, 3f);
        }
    }

    void Update()
    {
        if (transform.position.y < -100f)
        {
            Destroy(this);
        }
    }
}
