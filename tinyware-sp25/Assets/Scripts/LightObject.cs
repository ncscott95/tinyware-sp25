using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightObject : MonoBehaviour, IInteractable
{
    public bool IsActive;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D light2D;

    void Start()
    {
        SetActive(IsActive);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        if (IsActive)
        {
            spriteRenderer.color = Color.yellow;
            light2D.gameObject.SetActive(true);
        }
        else
        {
            spriteRenderer.color = Color.black;
            light2D.gameObject.SetActive(false);
        }
    }

    public void OnInteract()
    {
        if (IsActive) Destroy(gameObject);
    }
}
