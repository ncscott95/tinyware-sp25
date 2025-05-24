using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public bool IsActive;
    public List<LightObject> OnTrueObjects;
    public List<LightObject> OnFalseObjects;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Awake()
    {
        UpdateObjects();
    }

    public void OnInteract()
    {
        IsActive = !IsActive;
        UpdateObjects();

        if (IsActive)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void UpdateObjects()
    {
        foreach (LightObject light in OnTrueObjects) light.SetActive(IsActive);
        foreach (LightObject light in OnFalseObjects) light.SetActive(!IsActive);
    }
}
