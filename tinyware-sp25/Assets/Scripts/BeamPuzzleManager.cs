using System.Collections.Generic;
using UnityEngine;

public class BeamPuzzleManager : MonoBehaviour
{
    public static BeamPuzzleManager Instance;
    public List<LightBeam> Beams;
    public List<BeamLever> Levers;
    public LightObject EndLantern;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Beams[1].ToggleEnabled(false);
        Beams[2].ToggleEnabled(false);
    }

    public void BeamChainEffect()
    {
        for (int i = 1; i < Levers.Count; i++)
        {
            Beams[i].ToggleEnabled(Levers[i - 1].IsActive && Beams[i - 1].IsActive);
        }
        EndLantern.SetActive(Levers[^1].IsActive && Beams[^1].IsActive);
    }
}
