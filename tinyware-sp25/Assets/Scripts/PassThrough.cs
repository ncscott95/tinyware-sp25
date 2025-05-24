using UnityEngine;

public class PassThrough : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;

    void Start()
    {
        PlayerControls.OnLitChange += UpdatePassThrough;
    }

    void OnDisable()
    {
        PlayerControls.OnLitChange -= UpdatePassThrough;
    }

    private void UpdatePassThrough(bool value)
    {
        boxCollider.isTrigger = !value;
    }
}
