using UnityEngine;

public class PassThrough : MonoBehaviour
{
    [SerializeField] private Collider2D col2D;

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
        col2D.isTrigger = !value;
    }
}
