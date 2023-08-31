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
    [SerializeField]
    private float m_lerpingFactor = 0.5f;


    private void Awake()
    {
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
            //if (zoomAmount < i) // zoomAmount n�gatif, donc scroll backward
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

            //Vector3.Lerp();
            //Mathf.Lerp();
            // Pour le lerp faire en sorte que la distance peut �tre lerp � travers le updating

            //TODO: Lerp plut�t que d'effectuer imm�diatement la translation

        }
    }

    private void FixedUpdate()
    {
        MoveCameraInFrontOfObstructionsFUpdate();
    }

    private void MoveCameraInFrontOfObstructionsFUpdate()
    {
        // Raycast se fait dans le fixed Update car c'estb de la physique

        // Bit shift the index of the layer  (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;

        var vecteurDiff = transform.position - m_objectToLookAt.position;
        var distance = vecteurDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vecteurDiff, out hit, distance, layerMask))
        {
            // J'ai un oobjet entre mon focus et sa cam�ra
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff.normalized * hit.distance, Color.yellow);
            transform.SetPositionAndRotation(hit.point, transform.rotation);


        }
        else
        {
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
        if (m_distanceBetweenObjects > m_zoomScrollLimits.y)
            distance = m_zoomScrollLimits.y;
        if (m_distanceBetweenObjects < m_zoomScrollLimits.x)
            distance = m_zoomScrollLimits.x;

        return distance;
    }


}






























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
            //TODO: Faire une v�rification selon la distance la plus proche ou la plus �loign�e
            //Que je souhaite entre ma cam�ra et mon objet
            if (m_distanceBetweenObjects == m_zoomScrollLimits.x || m_distanceBetweenObjects == m_zoomScrollLimits.y)
                return;

            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);

            //if (m_distanceBetweenObjects > m_zoomScrollLimits.x && m_distanceBetweenObjects < m_zoomScrollLimits.y )
            //{
            //    
            //
            //    transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
            //}


            //TODO: Lerp plut�t que d'effectuer imm�diatement la translation
            
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