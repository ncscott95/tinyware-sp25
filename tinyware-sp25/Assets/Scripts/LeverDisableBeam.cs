using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[System.Serializable]
enum BeamDisableType
{
    DIM,
    DISABLE
}

public class LeverDisableBeam : MonoBehaviour, IInteractable
{
    public bool IsActive;
    [SerializeField] private BeamDisableType type;
    public List<LightBeam> LinkedLights;

    public void OnInteract()
    {
        IsActive = !IsActive;
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        if (type == BeamDisableType.DIM)
        {
            foreach (LightBeam beam in LinkedLights)
            {
                beam.TriggerDim();
            }
        }
        else if (type == BeamDisableType.DISABLE)
        {
            foreach (LightBeam beam in LinkedLights)
            {
                beam.ToggleEnabled(IsActive);
            }
        }
    }
}
