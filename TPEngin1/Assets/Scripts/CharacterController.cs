using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //STATE MACHINE
    private CharacterState m_currentState;
    private List<CharacterState> m_possibleStates;
    //STATE MACHINE
    // Le character controller est maintenent notre state machine
    // TODO changer le nom pour character controller state machine ou de quoi du genre

    // TODO important changer le nom pour STATE MACHINE CHARACTER CONTROLLER OR SOMETHING

    // Les variables deviennent des cSharp fields
    // fields en cSharp sont des mtéhodes qui nous permettent d'aller chercher de l'info d'ou la majuscule
    public Camera Camera { get; private set; }
    public Rigidbody RB { get; private set; }

    public Transform Transform { get; private set; }

    //[Header("Forward Movement")]
    [field: SerializeField]
    public float ForwardAccelerationValue { get; private set; }
    
    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }
    //[Header("Diagonal Movement")]
    
    [field: SerializeField]
    public float ForwardDiagonalsAccelerationValue { get; private set; }
    
    [field: SerializeField]
    public float MaxForwardDiagonalsVelocity { get; private set; }
    //[Header("Backward Movement")]
    
    [field: SerializeField]
    public float BackwardAccelerationValue { get; private set; }
    
    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; }
    //[Header("Strafe Movement")]
    
    [field: SerializeField]
    public float StrafeAccelerationValue { get; private set; }
    
    [field: SerializeField]
    public float MaxStrafeVelocity { get; private set; }
    //[Header("")]
    
    [field: SerializeField]
    public float DecelerationValue { get; private set; }

    [field: SerializeField]
    public float TurnSmoothTime { get; private set; }
    // Lower number means snappier turn
    public float TurnSmoothVelocity { get; private set; }


    private void Awake()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
    }



    void Start()
    {
        // Sans le serializeField on peut garder la référence privée, et on va chercher directement la caméra
        Camera = Camera.main;
        RB = GetComponent<Rigidbody>();
        Transform = GetComponent<Transform>();

        foreach(CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        // STATE MACHINE
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
        // STATE MACHINE
    }
    
    private void Update()
    {   
        // STATE MACHINE
        m_currentState.OnUpdate();
        // STATE MACHINE    
    }

    private void FixedUpdate()
    {
        // STATE MACHINE
        m_currentState.OnFixedUpdate();
        // STATE MACHINE


        
    }




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




