using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
enum BeamActionType
{
    DIM,
    DISABLE,
    ROTATE
}

public class BeamLever : MonoBehaviour, IInteractable
{
    public bool IsActive;
    [SerializeField] private BeamActionType type;
    public List<LightBeam> LinkedLights;
    public List<Transform> Targets;
    public int ID;

    public void OnInteract()
    {
        IsActive = !IsActive;
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        if (type == BeamActionType.DIM)
        {
            foreach (LightBeam beam in LinkedLights)
            {
                beam.TriggerDim();
            }
        }
        else if (type == BeamActionType.DISABLE)
        {
            foreach (LightBeam beam in LinkedLights)
            {
                beam.ToggleEnabled(IsActive);
            }
        }
        else if (type == BeamActionType.ROTATE)
        {
            for (int i = 0; i < LinkedLights.Count; i++)
            {
                LinkedLights[i].Target = IsActive ? Targets[0] : Targets[1];
                BeamPuzzleManager.Instance.BeamChainEffect();
            }
        }
    }
}
