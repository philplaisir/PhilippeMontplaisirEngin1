using UnityEngine;

public class CharacterController : MonoBehaviour
{ 
    private Camera m_camera;
    private Rigidbody m_rb;
    
    [SerializeField]
    private float m_forwardAccelerationValue = 10.0f;    
    [SerializeField]
    private float m_maxForwardVelocity = 10.0f;
    [SerializeField]
    private float m_forwardDiagonalsAccelerationValue = 10.0f;
    [SerializeField]
    private float m_maxForwardDiagonalsVelocity = 10.0f;
    [SerializeField]
    private float m_backwardAccelerationValue = 10.0f;
    [SerializeField]
    private float m_maxBackwardVelocity = 10.0f;
    [SerializeField]
    private float m_strafeAccelerationValue = 10.0f;
    [SerializeField]
    private float m_maxStrafeVelocity = 10.0f;
    [SerializeField]
    private float m_decelerationValue = 10.0f;

    





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
        //float cameraYRotation = m_camera.transform.rotation.eulerAngles.y;
        //transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);

        //Vector3 camDir = m_camera.transform.forward;
        //camDir = Vector3.ProjectOnPlane(camDir, Vector3.up);
        //transform.forward = camDir;


    }

    void FixedUpdate()
    {        







        
        // CHARACTER MOVEMENT RELATIVE TO CAMERA
        if (Input.anyKey)
        {
            if (IsTwoOrMoreReverseInputsInputedSimultaneouslyOne())
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


    

    Vector3 GetNormalizedVectorProjectedOnFloorForward()
    {
        Vector3 returnVector = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        returnVector.Normalize();
        return returnVector;
    }

    Vector3 GetNormalizedVectorProjectedOnFloorRight()
    {
        Vector3 returnVector = Vector3.ProjectOnPlane(m_camera.transform.right, Vector3.up);
        returnVector.Normalize();
        return returnVector;
    }

    

    void CharacterControllerDeceleration()
    {
        if (m_rb.velocity.magnitude > 0.1f)
        {
            Vector3 vector3 = m_rb.velocity.normalized;
            m_rb.AddForce(-vector3 * m_decelerationValue, ForceMode.Acceleration);
        }
    }



    // CHARACTER CONTROLLER RELATIVE TO CAMERA METHODS
    void CharacterControllerRelativeToCameraFUpdate()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            CharacterControllerRelativeToCameraMoveForwardLeft();
            //Vector3 cameraForward = m_camera.transform.forward;
            //cameraForward.y = 0.0f; // Ensure no rotation in the y-axis        
            //
            //// Rotate the character to face the same direction as the camera
            //if (cameraForward != Vector3.zero)
            //{
            //    transform.rotation = Quaternion.LookRotation(cameraForward);
            //}
            return;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            CharacterControllerRelativeToCameraMoveForwardRight();
            //Vector3 cameraForward = m_camera.transform.forward;
            //cameraForward.y = 0.0f; // Ensure no rotation in the y-axis        
            //
            //// Rotate the character to face the same direction as the camera
            //if (cameraForward != Vector3.zero)
            //{
            //    transform.rotation = Quaternion.LookRotation(cameraForward);
            //}
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            CharacterControllerRelativeToCameraMoveForward();            
        }
        if (Input.GetKey(KeyCode.S))
        {
            CharacterControllerRelativeToCameraMoveBackward();
            //Vector3 cameraForward = m_camera.transform.forward;
            //cameraForward.y = 0.0f; // Ensure no rotation in the y-axis        
            //
            //// Rotate the character to face the same direction as the camera
            //if (cameraForward != Vector3.zero)
            //{
            //    transform.rotation = Quaternion.LookRotation(-cameraForward);
            //}
        }
        if (Input.GetKey(KeyCode.A))
        {
            CharacterControllerRelativeToCameraStrafeLeft();
            //Vector3 cameraForward = m_camera.transform.forward;
            //cameraForward.y = 0.0f; // Ensure no rotation in the y-axis        
            //
            //// Rotate the character to face the same direction as the camera
            //if (cameraForward != Vector3.zero)
            //{
            //    transform.rotation = Quaternion.LookRotation(-cameraForward);
            //}
        }
        if (Input.GetKey(KeyCode.D))
        {
            CharacterControllerRelativeToCameraStrafeRight();
            //Vector3 cameraForward = m_camera.transform.forward;
            //cameraForward.y = 0.0f; // Ensure no rotation in the y-axis        
            //
            //// Rotate the character to face the same direction as the camera
            //if (cameraForward != Vector3.zero)
            //{
            //    transform.rotation = Quaternion.LookRotation(cameraForward);
            //}
        }
    }

    bool IsTwoOrMoreReverseInputsInputedSimultaneouslyOne()
    {
        // TODO à revérifier, ne fonctionne pas lrsqu'on fait WAS,WSD
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

        return returnValue;
    }

    void CharacterControllerRelativeToCameraMoveForward()
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloorForward();

        m_rb.AddForce(vectorProjectedOnFloorForward * m_forwardAccelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > m_maxForwardVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxForwardVelocity;
        }
    }

    void CharacterControllerRelativeToCameraMoveBackward()
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloorForward();

        m_rb.AddForce((vectorProjectedOnFloorForward * -1) * m_backwardAccelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > m_maxBackwardVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxBackwardVelocity;
        }
    }

    void CharacterControllerRelativeToCameraStrafeLeft()
    {
        Vector3 vectorProjectedOnFloorRight = GetNormalizedVectorProjectedOnFloorRight();

        m_rb.AddForce((vectorProjectedOnFloorRight * -1) * m_strafeAccelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > m_maxStrafeVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxStrafeVelocity;
        }
    }

    void CharacterControllerRelativeToCameraStrafeRight()
    {
        Vector3 vectorProjectedOnFloorRight = GetNormalizedVectorProjectedOnFloorRight();

        m_rb.AddForce(vectorProjectedOnFloorRight * m_strafeAccelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > m_maxStrafeVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxStrafeVelocity;
        }
    }

    void CharacterControllerRelativeToCameraMoveForwardLeft()
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloorForward();

        m_rb.AddForce(vectorProjectedOnFloorForward * m_forwardDiagonalsAccelerationValue, ForceMode.Acceleration);
        
        Vector3 vectorProjectedOnFloorRight = GetNormalizedVectorProjectedOnFloorRight();

        m_rb.AddForce((vectorProjectedOnFloorRight * -1) * m_forwardDiagonalsAccelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > m_maxForwardDiagonalsVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxForwardDiagonalsVelocity;
        }
    }

    void CharacterControllerRelativeToCameraMoveForwardRight()
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloorForward();

        m_rb.AddForce(vectorProjectedOnFloorForward * m_forwardDiagonalsAccelerationValue, ForceMode.Acceleration);

        Vector3 vectorProjectedOnFloorRight = GetNormalizedVectorProjectedOnFloorRight();

        m_rb.AddForce(vectorProjectedOnFloorRight * m_forwardDiagonalsAccelerationValue, ForceMode.Acceleration);

        if (m_rb.velocity.magnitude > m_maxForwardDiagonalsVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxForwardDiagonalsVelocity;
        }
    }

    // CHARACTER CONTROLLER RELATIVE TO CHARACTER METHODS




}
