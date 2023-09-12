using UnityEngine;

public class CharacterController : MonoBehaviour
{ 
    private Camera m_camera;
    private Rigidbody m_rb;

    [Header("Forward Movement")]
    [SerializeField]
    private float m_forwardAccelerationValue = 10.0f;    
    [SerializeField]
    private float m_maxForwardVelocity = 10.0f;
    [Header("Diagonal Movement")]
    [SerializeField]
    private float m_forwardDiagonalsAccelerationValue = 10.0f;
    [SerializeField]
    private float m_maxForwardDiagonalsVelocity = 10.0f;
    [Header("Backward Movement")]
    [SerializeField]
    private float m_backwardAccelerationValue = 10.0f;
    [SerializeField]
    private float m_maxBackwardVelocity = 10.0f;
    [Header("Strafe Movement")]
    [SerializeField]
    private float m_strafeAccelerationValue = 10.0f;
    [SerializeField]
    private float m_maxStrafeVelocity = 10.0f;
    [Header("")]
    [SerializeField]
    private float m_decelerationValue = 10.0f;

    [SerializeField]
    private float m_turnSmoothTime = 0.5f; // Lower number means snappier turn
    private float m_turnSmoothVelocity;



    // Start is called before the first frame update
    void Start()
    {
        // Sans le serializeField on peut garder la référence privée, et on va chercher directement la caméra
        m_camera = Camera.main;
        m_rb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void FixedUpdate()
    {              
        // CHARACTER MOVEMENT RELATIVE TO CAMERA
        if (Input.anyKey)
        {
            if (IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera())
            {
                CharacterControllerDeceleration();
                return;
            }
            CharacterControllerRelativeToCameraFUpdate();
        }
        else
        {
            CharacterControllerDeceleration();
        }         

        // TODO 230831
        // Apliquer les déplacements relatifs à la caméra dans les 3 autres directions
        // Avoir des vitesse de déplacement différentes maximales vers les côtés et vers l'arrière
        // Lorsqu'aucun input est détecté décélérer le personnage rapidement

        // TODO 230831
        // Essayer d'implémenter d'autres types de déplacements (relatif au personnag, tank control)
        // Essayer d'ajouter contrôle avec manette
        
        Debug.Log(m_rb.velocity.magnitude);
    }

    private Vector3 GetNormalizedVectorProjectedOnFloor(Vector3 direction)
    {
        Vector3 projectedVector;

        if (direction == Vector3.forward)
        {
            projectedVector = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        }
        else if (direction == Vector3.right)
        {
            projectedVector = Vector3.ProjectOnPlane(m_camera.transform.right, Vector3.up);
        }
        else
        {
            Debug.LogWarning("Invalid direction! Must be Vector3.forward or Vector3.right.");
            return Vector3.zero; // Return some default value or handle the error accordingly
        }

        projectedVector.Normalize();
        return projectedVector;
    }    

    private void CharacterControllerDeceleration()
    {
        if (m_rb.velocity.magnitude > 0.1f)
        {
            Vector3 vector3 = m_rb.velocity.normalized;
            m_rb.AddForce(-vector3 * m_decelerationValue, ForceMode.Acceleration);
        }
    }



    // CHARACTER CONTROLLER RELATIVE TO CAMERA METHODS
    private void CharacterControllerRelativeToCameraFUpdate()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCameraDiagonals(m_forwardDiagonalsAccelerationValue, m_maxForwardDiagonalsVelocity, -1);
            return;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCameraDiagonals(m_forwardDiagonalsAccelerationValue, m_maxForwardDiagonalsVelocity, 1);
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.forward, m_forwardAccelerationValue, m_maxForwardVelocity, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.forward, m_backwardAccelerationValue, m_maxBackwardVelocity, -1);            
        }
        if (Input.GetKey(KeyCode.A))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.right, m_strafeAccelerationValue, m_maxStrafeVelocity, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.right, m_strafeAccelerationValue, m_maxStrafeVelocity, 1);
        }
    }

    private bool IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera()
    {
        // TODO à revérifier, ne fonctionne pas lorsqu'on fait WAS,WSD
        bool returnValue = false;

        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)))
        {
            returnValue = true;
        }

        return returnValue;
    }

    private void ReorientCharacterTowardsChameraDirection()
    {
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, m_camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_turnSmoothTime);
    }

    private void CharacterControllerRelativeToCamera(Vector3 direction, float accelerationValue, float maxVelocity, int isVectorReversed)
    {        
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloor(direction);

        m_rb.AddForce((vectorProjectedOnFloorForward * isVectorReversed) * accelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > maxVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= maxVelocity;
        }
    }

    private void CharacterControllerRelativeToCameraDiagonals(float accelerationValue, float maxVelocity, int isVectorReversed)
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloor(Vector3.forward);
        m_rb.AddForce(vectorProjectedOnFloorForward * accelerationValue, ForceMode.Acceleration);

        Vector3 vectorProjectedOnFloorRight = GetNormalizedVectorProjectedOnFloor(Vector3.right);
        m_rb.AddForce((vectorProjectedOnFloorRight * isVectorReversed) * accelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > maxVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= maxVelocity;
        }
    }   

    

    // CHARACTER CONTROLLER RELATIVE TO CHARACTER METHODS




}






































// ARCHIVE

//float cameraYRotation = m_camera.transform.rotation.eulerAngles.y;
//transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);

//Vector3 camDir = m_camera.transform.forward;
//camDir = Vector3.ProjectOnPlane(camDir, Vector3.up);
//transform.forward = camDir;

//Vector3 cameraForward = m_camera.transform.forward;
//cameraForward.y = 0.0f; // Ensure no rotation in the y-axis        
//
//// Rotate the character to face the same direction as the camera
//if (cameraForward != Vector3.zero)
//{
//    transform.rotation = Quaternion.LookRotation(cameraForward);
//}

//if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) ||
//    (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)))
//{
//    returnValue = true;
//}
//if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)) ||
//    (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)))
//{
//    returnValue = true;
//}


//void CharacterControllerRelativeToCameraMoveForward()
//{
    //float inputX = Input.GetAxis("Horizontal");
    //float inputY = Input.GetAxis("Vertical");
    //
    //Vector2 input = new Vector2(inputX, inputY);
    //Vector2 inputDirection = input.normalized;
    //
    //if(inputDirection != Vector2.zero)
    //{
    //    float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
    //    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref m_turnSmoothVelocity, m_turnSmoothTime);
    //
    //}

    //float inputX = m_camera.GetAxis("Horizontal");
    //float inputY = m_camera.GetAxis("Vertical");

    //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, m_camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_turnSmoothTime);
    //transform.eulerAngles = Vector3.up * m_camera.transform.eulerAngles.y;




