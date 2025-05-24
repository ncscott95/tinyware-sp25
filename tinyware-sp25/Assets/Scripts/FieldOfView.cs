using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float angleIncrement = 2f;
    [SerializeField] LayerMask layerMask;
    private Light2D light2D;
    private float fov;
    private float viewDistance;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        fov = light2D.pointLightInnerAngle;
        viewDistance = light2D.pointLightOuterRadius;
    }

    void Update()
    {
        int rayCount = (int)(fov / angleIncrement);
        float angle = -fov;


        for (int i = 0; i <= rayCount; i++)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider != null)
            {
                // Hit
                if (raycastHit2D.collider.GetComponent<PlayerControls>() != null)
                {
                    Debug.Log("hit");
                }
            }
            else
            {
                // No Hit
                Debug.Log("no hit");
            }

            angle -= angleIncrement; // "-=" because going clock-wise
        }
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
