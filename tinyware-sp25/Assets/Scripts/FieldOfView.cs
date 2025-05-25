using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldOfView : MonoBehaviour
{
    public bool isHittingPlayer;
    [SerializeField] float angleIncrement = 5f;
    [SerializeField] LayerMask layerMask;
    private Light2D light2D;
    private float fov;
    private float viewDistance;

    // Debug
    public GameObject debugGO;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        fov = light2D.pointLightInnerAngle;
        viewDistance = light2D.pointLightOuterRadius;
    }

    void Update()
    {
        if (light2D.color.a == 0f)
        {
            isHittingPlayer = false;
            return;
        }

        int rayCount = (int)(fov / angleIncrement);
        float angle = -90f + (fov / 2f);

        int numHits = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider != null)
            {
                // Hit
                if (raycastHit2D.collider.GetComponent<PlayerControls>() != null)
                {
                    numHits++;
                    if (light2D.intensity >= PlayerControls.Instance.lightDeathThreshold)
                    {
                        // Player dies
                        PlayerControls.Instance.Death();
                    }
                }
                vertex = raycastHit2D.point;
            }
            else
            {
                vertex = transform.position + GetVectorFromAngle(angle) * viewDistance;
            }

            angle -= angleIncrement; // "-=" because going clock-wise
            // Instantiate(debugGO, vertex, Quaternion.identity);
        }


        isHittingPlayer = numHits >= 3;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
