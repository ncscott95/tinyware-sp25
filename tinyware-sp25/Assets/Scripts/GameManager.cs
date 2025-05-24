using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LightObject> LightObjects = new();

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
        LightObjects = FindObjectsByType<LightObject>(FindObjectsSortMode.None).ToList();
    }

    void Update()
    {
        bool playerHit = false;
        foreach (var light in LightObjects)
        {
            if (light.fov.isHittingPlayer)
            {
                playerHit = true;
                break;
            }
        }

        PlayerControls.Instance.IsLit = playerHit;
    }
}
