using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightObject : MonoBehaviour, IInteractable
{
    public bool IsActive;
    public FieldOfView fov;
    [SerializeField] private Light2D light2D;

    void Start()
    {
        SetActive(IsActive);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        Color tempCol = light2D.color;

        if (IsActive)
        {
            tempCol.a = 1f;
        }
        else
        {
            tempCol.a = 0f;
        }

        light2D.color = tempCol;
    }

    public void OnInteract()
    {
        if (IsActive) SetActive(false);
    }
}
