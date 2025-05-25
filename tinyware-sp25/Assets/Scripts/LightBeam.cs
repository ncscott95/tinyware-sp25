using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBeam : MonoBehaviour
{
    public Transform Target;
    private Light2D light2D;
    private Vector3[] newShapePath;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        PointAtTarget();
    }

    void Update()
    {
        PointAtTarget();
    }

    private void PointAtTarget()
    {
        Vector3 offset = Target.position - transform.position;
        float distance = offset.magnitude;
        newShapePath = new Vector3[] {
            new(-0.5f, 0.5f, 0f),
            new(-1f, 0f, 0f),
            new(-0.5f, -0.5f, 0f),
            new(distance, -0.5f, 0f),
            new(distance, 0.5f, 0f)
        };
        light2D.SetShapePath(newShapePath);
        transform.right = offset;
    }
}
