using UnityEngine;

public class LightObject : MonoBehaviour, IInteractable
{
    public bool IsActive;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetActive(bool active)
    {
        IsActive = active;
        if (IsActive)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.black;
        }
    }

    public void OnInteract()
    {
        if (IsActive) Destroy(gameObject);
    }
}
