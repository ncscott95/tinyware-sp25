using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class LightBeam : MonoBehaviour
{
    public Transform Target;
    public float DimRecoverTime;
    private Light2D light2D;
    private PolygonCollider2D polygonCollider2D;
    private Vector3[] newShapePath;
    private Vector2[] newColliderPath;
    private float initialIntensity;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        initialIntensity = light2D.intensity;
        polygonCollider2D = GetComponent<PolygonCollider2D>();
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
        newColliderPath = new Vector2[] {
            new(-0.5f, 0.5f),
            new(-0.5f, -0.5f),
            new(distance, -0.5f),
            new(distance, 0.5f)
        };
        light2D.SetShapePath(newShapePath);
        polygonCollider2D.SetPath(0, newColliderPath);
        transform.right = offset;
    }

    public void TriggerDim()
    {
        polygonCollider2D.enabled = false;
        LeanTween.value(0f, initialIntensity, DimRecoverTime).setEaseInExpo().setOnUpdate((float val) => { light2D.intensity = val; }).setOnComplete(() => polygonCollider2D.enabled = true);
    }

    public void ToggleEnabled(bool enabled)
    {
        light2D.intensity = enabled ? initialIntensity : 0f;
        polygonCollider2D.enabled = enabled;
    }
}
