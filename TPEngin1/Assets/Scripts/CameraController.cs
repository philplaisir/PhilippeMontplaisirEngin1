using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO finir le lerp du scroll
    // suivre le personnage avec la caméra

    private Collider m_cameraCollider;

    //[Header("Header")] 
    [SerializeField]
    private Transform m_objectToLookAt;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private Vector2 m_clampingXRotationValues = Vector2.zero;
    [SerializeField]
    private float m_desiredDistanceFromTarget = 5.0f;
    [SerializeField]
    private float m_distanceFromTargetIfTouchingWall = 0.0f;
    private bool m_wallBetweenTargetAndDesiredCameraPosition = false;
    [SerializeField]
    private float m_cameraLerpSpeed = 1.0f;

    [Header("Camera Zoom")]
    [SerializeField]
    private Vector2 m_zoomScrollLimits = Vector2.zero; // x == minimum, y == maximum    
    [SerializeField]
    private float m_scrollSpeed = 0.001f;
    private float m_endScrollDistance = 0.0f;
    private float m_elapsedDistance = 0.0f;
    private float m_projectedDistanceFromTarget = 0.0f;
    private float m_startDistanceFromTarget = 0.0f;
    private bool m_lerping = false;

    private float m_savedDistance = 0.0f;



    private void Awake()
    {
        m_cameraCollider = GetComponent<Collider>();
        m_savedDistance = m_desiredDistanceFromTarget;
    }
    
    private void LateUpdate()
    {        
        UpdateHorizontalMovements();
        UpdateVerticalMovements();        
        UpdateCameraScroll();        
    }   

    private void UpdateHorizontalMovements()
    {
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);
    }

    private void UpdateVerticalMovements()
    {
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        float eulersAngleX = transform.rotation.eulerAngles.x;

        float comparisonAngle = eulersAngleX + currentAngleY;

        comparisonAngle = ClampAngle(comparisonAngle);

        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x)
            || (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }
        transform.RotateAround(m_objectToLookAt.position, transform.right, currentAngleY);
    }

    private void UpdateCameraPositionFUpdate()
    {
        
        //transform.position = m_objectToLookAt.position - transform.forward * m_distanceFromTargetIfTouchingWall;

        //transform.position = m_objectToLookAt.position - transform.forward * m_desiredDistanceFromTarget;

        //if(m_wallBetweenTargetAndDesiredCameraPosition)
        //{
        //    Vector3 targetPositionWall = m_objectToLookAt.position - transform.forward * m_distanceFromTargetIfTouchingWall;
        //    transform.position = Vector3.Lerp(transform.position, targetPositionWall, m_cameraLerpSpeed * Time.deltaTime);
        //    return;
        //}


        Vector3 targetPosition = m_objectToLookAt.position - transform.forward * m_desiredDistanceFromTarget;
        transform.position = Vector3.Lerp(transform.position, targetPosition, m_cameraLerpSpeed * Time.deltaTime);
        m_savedDistance = m_desiredDistanceFromTarget;
    }

    private void UpdateCameraScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            m_endScrollDistance -= Input.mouseScrollDelta.y;
            if((m_desiredDistanceFromTarget == m_zoomScrollLimits.x && m_endScrollDistance < 0) ||
                (m_desiredDistanceFromTarget == m_zoomScrollLimits.y && m_endScrollDistance > 0))
            {
                m_endScrollDistance = 0;
                return;
            }            
            m_elapsedDistance = 0.0f;
            m_projectedDistanceFromTarget = Mathf.RoundToInt(m_desiredDistanceFromTarget + m_endScrollDistance);
            m_startDistanceFromTarget = m_desiredDistanceFromTarget;
            m_lerping = true;
        }
        
        if (m_lerping && m_endScrollDistance > 0)
        {
            m_elapsedDistance += m_scrollSpeed;
            float percentageComplete = m_elapsedDistance / m_endScrollDistance;            

            float distanceToAdd = Mathf.Lerp(0, m_endScrollDistance, percentageComplete * Time.deltaTime);           

            m_desiredDistanceFromTarget += distanceToAdd;
            
            if (m_projectedDistanceFromTarget <= m_desiredDistanceFromTarget)
            {
                m_desiredDistanceFromTarget = m_projectedDistanceFromTarget;
                m_lerping = false;
                m_endScrollDistance = 0.0f;
                m_startDistanceFromTarget = 0.0f;
                return;
            }            
        }

        if (m_lerping && m_endScrollDistance < 0)
        {
            m_elapsedDistance += m_scrollSpeed;
            float percentageComplete = m_elapsedDistance / -m_endScrollDistance;            

            float distanceToAdd = Mathf.Lerp(0, -m_endScrollDistance, percentageComplete * Time.deltaTime);

            m_desiredDistanceFromTarget -= distanceToAdd;

            if (m_projectedDistanceFromTarget >= m_desiredDistanceFromTarget)
            {
                m_desiredDistanceFromTarget = m_projectedDistanceFromTarget;
                m_lerping = false;
                m_endScrollDistance = 0.0f;
                m_startDistanceFromTarget = 0.0f;
                return;
            }
        }

        m_desiredDistanceFromTarget = ClampZoom(m_desiredDistanceFromTarget);
        m_savedDistance = m_desiredDistanceFromTarget;
        
    }

    private void FixedUpdate()
    {
        UpdateCameraPositionFUpdate();
        MoveCameraInFrontOfObstructionsFUpdate();
    }

    private void MoveCameraInFrontOfObstructionsFUpdate()
    {
        // Raycast se fait dans le fixed Update car c'est de la physique

        // Bit shift the index of the layer  (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;

        var vecteurDiff = transform.position - m_objectToLookAt.position;
        var distance = vecteurDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vecteurDiff, out hit, distance, layerMask))
        {
            
            m_wallBetweenTargetAndDesiredCameraPosition = true;
            // J'ai un objet entre mon focus et sa caméra
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff.normalized * hit.distance, Color.yellow);
            transform.SetPositionAndRotation(hit.point, transform.rotation);

            var vecteurDiff2 = hit.point - m_objectToLookAt.position;

            m_desiredDistanceFromTarget = vecteurDiff2.z;
            m_distanceFromTargetIfTouchingWall = vecteurDiff2.z;


            Debug.Log("Valeurs du vecteur" + vecteurDiff2);
            
            
            

            // TODO changer la logique ici, sûrement enclancher un nouveau m_distanceFromTarget avec un bool ou de quoi

        }
        else
        {
            m_wallBetweenTargetAndDesiredCameraPosition = false;
            m_desiredDistanceFromTarget = m_savedDistance;
            //J'en ai pas
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff, Color.white);

        }
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

    private float ClampZoom(float distance)
    {
        if (m_desiredDistanceFromTarget > m_zoomScrollLimits.y)
            distance = m_zoomScrollLimits.y;
        if (m_desiredDistanceFromTarget < m_zoomScrollLimits.x)
            distance = m_zoomScrollLimits.x;

        return distance;
    }


}



// Ça marche au pire
//if (Input.mouseScrollDelta.y != 0)
//{
//    m_distanceFromTarget -= Input.mouseScrollDelta.y;
//
//    m_distanceFromTarget = ClampZoom(m_distanceFromTarget);
//}

//Vector3.Lerp();
//Mathf.Lerp();
// Pour le lerp faire en sorte que la distance peut être lerp à travers le updating

//TODO: Lerp plutôt que d'effectuer immédiatement la translation



