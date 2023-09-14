using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO finir le lerp du scroll
    // suivre le personnage avec la caméra

    private Collider m_cameraCollider;


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

    // Update is called once per frame
    void LateUpdate()
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
        // Ça marche en ce moment
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

















































// ARCHIVE








//// Pour le lerp changer la variable, ensuite la lerpedDistance dans un variable qui va tranquillement incrémenter m_distanceFromTarget
//
//float lerpSpeed = 0.1f; // Adjust this value to control the speed of the lerp
//float currentDistance = Vector3.Distance(transform.position, m_objectToLookAt.position);
//
//float lerpedDistance = Mathf.Lerp(currentDistance, m_distanceFromTarget, lerpSpeed);
//Vector3 direction = (transform.position - m_objectToLookAt.position).normalized;
//Vector3 lerpedPosition = m_objectToLookAt.position + direction * lerpedDistance;
//
//transform.position = lerpedPosition;
//
//// Make sure the camera is still looking at the target
//transform.LookAt(m_objectToLookAt);



// Première manière de scroll qui a marché pas pire
//
//if (Input.mouseScrollDelta.y != 0)
//{
//    m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
//    m_distanceBetweenObjects = ClampZoom(m_distanceBetweenObjects);
//
//    if (m_distanceBetweenObjects == m_zoomScrollLimits.x)
//    {
//        if (Input.mouseScrollDelta.y > 0)
//        {
//            return;
//        }
//        else
//        {
//            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
//            return;        
//        }
//    }
//
//    if (m_distanceBetweenObjects == m_zoomScrollLimits.y)
//    {
//        if (Input.mouseScrollDelta.y < 0)
//        {
//            return;
//        }
//        else
//        {
//            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
//            return;
//        }
//    }
//    transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);        
//}

// Deuxième iétration du scroll avec les nouvelles variables
//if(Input.mouseScrollDelta.y != 0)
//{
//    if (m_distanceFromTarget == m_zoomScrollLimits.x)
//    {
//        if (Input.mouseScrollDelta.y > 0)
//        {
//            return;
//        }
//        else
//        {
//            m_distanceFromTarget -= Input.mouseScrollDelta.y;
//            return;        
//        }
//    }
//    if (m_distanceFromTarget == m_zoomScrollLimits.y)
//    {
//        if (Input.mouseScrollDelta.y < 0)
//        {
//            return;
//        }
//        else
//        {
//            m_distanceFromTarget -= Input.mouseScrollDelta.y;
//            return;
//        }
//    }            
//    m_distanceFromTarget -= Input.mouseScrollDelta.y;
//}

// J'ai changé pour mon propre clamp
//m_distanceFromTarget = Mathf.Clamp(m_distanceFromTarget, m_zoomScrollLimits.x, m_zoomScrollLimits.y);











//if (Input.mouseScrollDelta.y != 0)
//{
//    m_originalDistanceBetweenCameraAndTarget = Vector3.Distance(m_objectToLookAt.position, transform.position);
//    m_originalScrollInput = Input.mouseScrollDelta.y;
//    m_endDistance = m_originalDistanceBetweenCameraAndTarget + m_originalScrollInput;
//}
//
//m_originalScrollInputCopy = m_originalScrollInput;
//m_originalScrollInput -= 0.01f;
//
////float percentageComplete = m_originalScrollInput / m_originalScrollInputCopy;
//
//float percentageComplete = Vector3.Distance(m_objectToLookAt.position, transform.position) / m_endDistance;
//
//float lerpedFloat = Mathf.Lerp(1, 0, percentageComplete);
//
//transform.Translate(Vector3.forward * lerpedFloat, Space.Self);


//Vector3.Distance(m_objectToLookAt.position, transform.position)




//if (Input.mouseScrollDelta.y != 0)
//{
//    m_originalScrollInput += Input.mouseScrollDelta.y;
//    m_originalDistanceBetweenCameraAndTarget = Vector3.Distance(m_objectToLookAt.position, transform.position);
//    m_endDistance = m_originalScrollInput + m_originalDistanceBetweenCameraAndTarget;
//    m_originalDistanceBetweenStartAndEnd = m_originalDistanceBetweenCameraAndTarget - m_endDistance;
//    Debug.Log("");
//}




//if (Input.mouseScrollDelta.y != 0)
//{
//    m_originalScrollInput += Input.mouseScrollDelta.y;
//    m_originalDistanceBetweenCameraAndTarget = Vector3.Distance(m_objectToLookAt.position, transform.position);
//    m_endDistance = m_originalScrollInput + m_originalDistanceBetweenCameraAndTarget;
//    m_originalDistanceBetweenStartAndEnd = m_originalDistanceBetweenCameraAndTarget - m_endDistance;
//    Debug.Log("");
//}
//
//if (Vector3.Distance(m_objectToLookAt.position, transform.position) == m_endDistance)
//{
//    return;
//}
//
//m_distanceTraveled += 0.01f;
//float percentageComplete = m_distanceTraveled / m_originalDistanceBetweenStartAndEnd;
//
//float lerpedFloat = Mathf.Lerp(m_originalDistanceBetweenStartAndEnd, m_endDistance, percentageComplete);
//
//transform.Translate(Vector3.forward * lerpedFloat, Space.Self);
//
//Debug.Log(Vector3.Distance(m_objectToLookAt.position, transform.position));
//transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);








//float zoomChangeAmount = 10.0f;

//if (Input.mouseScrollDelta.y != 0)
//{
//    m_zoomChangeAmount += Input.mouseScrollDelta.y;
//}
//
//if (m_zoomChangeAmount != 0)
//{
//
//}




//if (Input.mouseScrollDelta.y != 0)
//{
//    m_scrollDistanceToTravel += Input.mouseScrollDelta.y;
//}
//
//float targetDistance = m_distanceBetweenCameraAndTarget + m_scrollDistanceToTravel;
//m_distanceBetweenCameraAndTarget = Mathf.Lerp(m_distanceBetweenCameraAndTarget, targetDistance, Time.deltaTime);
//
//Vector3 direction = transform.position - m_objectToLookAt.position;
//direction.Normalize();
//transform.position = m_objectToLookAt.position + direction * m_distanceBetweenCameraAndTarget;


//if (Input.mouseScrollDelta.y != 0)
//{            
//    m_scrollDistanceToTravel += Input.mouseScrollDelta.y;
//    m_distanceBetweenCameraAndTarget = Vector3.Distance(m_objectToLookAt.position, transform.position);
//    m_endDistance = m_scrollDistanceToTravel + m_distanceBetweenCameraAndTarget;
//}
//
//float percentageComplete = m_endDistance / Vector3.Distance(m_objectToLookAt.position, transform.position);
//float lerpedFloat = Mathf.Lerp(m_distanceBetweenCameraAndTarget, m_endDistance, percentageComplete);
//transform.Translate(Vector3.forward * lerpedFloat, Space.Self);

//m_distanceBetweenCameraAndTarget = Vector3.Distance(m_objectToLookAt.position, transform.position);
//float targetDistance = m_distanceBetweenCameraAndTarget + m_scrollDistanceToTravel;
//m_distanceBetweenCameraAndTarget = Mathf.Lerp(m_distanceBetweenCameraAndTarget, targetDistance, Time.deltaTime);
//transform.Translate(Vector3.forward * m_distanceBetweenCameraAndTarget, Space.Self);

//if (m_scrollDistanceToTravel < 0 && m_scrollDistanceToTravel > 0)
//{
//}

















//float zoomAmount = Input.mouseScrollDelta.y;
//
//
//
//
//var i = 0.0f;
//
//if (zoomAmount > i) // zoomAmount positif, donc scroll forward
//{
//    while (i < zoomAmount)
//    {
//        i += Time.deltaTime * m_lerpingFactor;
//        transform.Translate(Vector3.forward * (Mathf.Lerp(0, zoomAmount, i)), Space.Self);
//
//    }
//
//}
//if (zoomAmount < i) // zoomAmount négatif, donc scroll backward
//{
//    while (i > zoomAmount)
//    {
//        i -= Time.deltaTime * m_lerpingFactor;
//        transform.Translate(Vector3.forward * (Mathf.Lerp(zoomAmount, 0, i)), Space.Self);
//
//    }
//
//}


//while (i < Input.mouseScrollDelta.y)            
//{ 
//    i += Time.deltaTime * m_lerpingFactor;
//    transform.Translate(Vector3.forward * (Mathf.Lerp(0, Input.mouseScrollDelta.y, i)), Space.Self);
//
//}











/*
 Vector3 targetPosition;

            m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
            m_distanceBetweenObjects = ClampZoom(m_distanceBetweenObjects);

            float lerpFactor = 0.2f; // You can adjust this value for the desired smoothness

            if (m_distanceBetweenObjects == m_zoomScrollLimits.x)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    return;
                }
                else
                {
                    targetPosition = transform.position + Vector3.forward * Input.mouseScrollDelta.y;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, lerpFactor);
                    return;
                }
            }

            if (m_distanceBetweenObjects == m_zoomScrollLimits.y)
            {
                if (Input.mouseScrollDelta.y < 0)
                {
                    return;
                }
                else
                {
                    targetPosition = transform.position + Vector3.forward * Input.mouseScrollDelta.y;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, lerpFactor);
                    return;
                }
            }

            Vector3 translation = Vector3.forward * Input.mouseScrollDelta.y;
            targetPosition = transform.position + translation;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpFactor);









            /*

            Vector3 targetPosition;
            Vector3 translation;
            Vector3 lerpedPosition;

            m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
            m_distanceBetweenObjects = ClampZoom(m_distanceBetweenObjects);




            if (m_distanceBetweenObjects == m_zoomScrollLimits.x)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    return;
                }
                else
                {
                    translation = Vector3.forward * Input.mouseScrollDelta.y;

                    // Calculate the target position for translation
                    targetPosition = transform.position + translation;

                    // Apply lerp to the target position
                    lerpedPosition = Vector3.Lerp(transform.position, targetPosition, m_lerpingFactor);

                    // Apply the translation using the lerped position
                    transform.Translate(lerpedPosition - transform.position, Space.Self);
                    return;
                    
                }
            }
            
            if (m_distanceBetweenObjects == m_zoomScrollLimits.y)
            {
                if (Input.mouseScrollDelta.y < 0)
                {
                    return;
                }
                else
                {
                    translation = Vector3.forward * Input.mouseScrollDelta.y;

                    // Calculate the target position for translation
                    targetPosition = transform.position + translation;

                    // Apply lerp to the target position
                    lerpedPosition = Vector3.Lerp(transform.position, targetPosition, m_lerpingFactor);

                    // Apply the translation using the lerped position
                    transform.Translate(lerpedPosition - transform.position, Space.Self);
                    return;
            
                    
                }
            }



            translation = Vector3.forward * Input.mouseScrollDelta.y;

            // Calculate the target position for translation
            targetPosition = transform.position + translation;

            // Apply lerp to the target position
            lerpedPosition = Vector3.Lerp(transform.position, targetPosition, m_lerpingFactor);

            // Apply the translation using the lerped position
            transform.Translate(lerpedPosition - transform.position, Space.Self);


            // Essayer de 



            */





//Vector3 translation = Vector3.forward * Input.mouseScrollDelta.y;
//targetPosition = transform.position + translation;
//transform.position = Vector3.Lerp(transform.position, targetPosition, m_lerpingFactor);







/*



if (Input.mouseScrollDelta.y != 0)
{     


m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
m_distanceBetweenObjects = ClampZoom(m_distanceBetweenObjects);




if (m_distanceBetweenObjects == m_zoomScrollLimits.x)
{
    if (Input.mouseScrollDelta.y > 0)
    {
        return;
    }
    else
    {
        transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
        return;

    }
}

if (m_distanceBetweenObjects == m_zoomScrollLimits.y)
{
    if (Input.mouseScrollDelta.y < 0)
    {
        return;
    }
    else
    {
        transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
        return;


    }
}

transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);










































 m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
m_distanceBetweenObjects = ClampZoom(m_distanceBetweenObjects);

Vector3 translation = Vector3.forward * Input.mouseScrollDelta.y;
Vector3 targetPosition = transform.position + translation;

// Use lerp to smooth the translation
float lerpFactor = 0.2f; // You can adjust this value for the desired smoothness
Vector3 lerpedPosition = Vector3.Lerp(transform.position, targetPosition, lerpFactor);

// Update the camera's position
transform.position = lerpedPosition;




transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * lerpedPosition.y, Space.Self);
        return;









 */






















/*
 
 using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform m_objectToLookAt;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private Vector2 m_clampingXRotationValues = Vector2.zero;
    [SerializeField]
    private Vector2 m_zoomScrollLimits = Vector2.zero;
    [SerializeField]
    private float m_distanceBetweenObjects;

    private void Awake()
    {
        m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
    }

    // Update is called once per frame
    void Update()
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

    private void UpdateCameraScroll()
    {

        

        if (Input.mouseScrollDelta.y != 0)
        {
            m_distanceBetweenObjects = Vector3.Distance(m_objectToLookAt.position, transform.position);
            m_distanceBetweenObjects = ClampZoom(m_distanceBetweenObjects);
            //TODO: Faire une vérification selon la distance la plus proche ou la plus éloignée
            //Que je souhaite entre ma caméra et mon objet
            if (m_distanceBetweenObjects == m_zoomScrollLimits.x || m_distanceBetweenObjects == m_zoomScrollLimits.y)
                return;

            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);

            //if (m_distanceBetweenObjects > m_zoomScrollLimits.x && m_distanceBetweenObjects < m_zoomScrollLimits.y )
            //{
            //    
            //
            //    transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
            //}


            //TODO: Lerp plutôt que d'effectuer immédiatement la translation
            
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
        if (m_distanceBetweenObjects > m_zoomScrollLimits.y)
            distance = m_zoomScrollLimits.y;
        if (m_distanceBetweenObjects < m_zoomScrollLimits.x)
            distance = m_zoomScrollLimits.x;

        return distance;
    }


}
 
 
 
 
 */