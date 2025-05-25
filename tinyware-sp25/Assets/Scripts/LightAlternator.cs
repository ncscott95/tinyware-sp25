using System.Collections.Generic;
using UnityEngine;

public class LightAlternator : MonoBehaviour
{
    public List<LightObject> Lights;
    public List<List<int>> States = new()
    {
        new() {0, 1, 1, 1, 1, 1},
        new() {0, 0, 1, 1, 1, 1},
        new() {1, 0, 1, 1, 1, 1},
        new() {0, 0, 1, 1, 1, 1},
        new() {0, 1, 1, 1, 1, 1},
        new() {0, 0, 1, 1, 1, 1},
        new() {1, 0, 1, 1, 1, 1},
        new() {1, 0, 0, 1, 1, 1},
        new() {1, 1, 0, 1, 1, 1},
        new() {1, 1, 0, 0, 1, 1},
        new() {1, 1, 1, 0, 1, 1},
        new() {1, 1, 0, 0, 1, 1},
        new() {1, 1, 0, 1, 1, 1},
        new() {1, 1, 0, 0, 1, 1},
        new() {1, 1, 1, 0, 1, 1},
        new() {1, 1, 0, 0, 1, 1},
        new() {1, 1, 0, 1, 1, 1},
        new() {1, 1, 0, 0, 1, 1},
        new() {1, 1, 1, 0, 1, 1},
        new() {1, 1, 1, 0, 0, 1},
        new() {1, 1, 1, 1, 0, 1},
        new() {1, 1, 1, 1, 0, 0},
        new() {1, 1, 1, 1, 1, 0}
    };
    private int index = 0;
    public float StepTime;
    private float timer;

    void Start()
    {
        timer = StepTime;
        ToggleLights();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) ToggleLights();
    }

    private void ToggleLights()
    {
        timer = StepTime;
        for (int i = 0; i < 6; i++)
        {
            Lights[i].SetActive(States[index][i] == 1);
        }
        index = (index + 1) % States.Count;
    }
}
