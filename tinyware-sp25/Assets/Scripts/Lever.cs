using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public bool IsActive;
    public List<LightObject> LinkedLights;
    [SerializeField] private SpriteRenderer spriteRenderer;

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
        foreach (LightObject light in LinkedLights) light.SetActive(!light.IsActive);
    }
}
