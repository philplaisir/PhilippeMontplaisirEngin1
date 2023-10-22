using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerSM : BaseStateMachine<CharacterState>
{
    [field: SerializeField] private Animator Animator { get; set; }

    [SerializeField] private CharacterFloorTrigger m_floorTrigger;
    

    public Camera Camera { get; private set; }
    public Transform Transform { get; private set; }

    public bool IsHit { get; set; }                  
    
    [field: SerializeField] public Rigidbody RB { get; private set; }
    [field: SerializeField] public GameObject MainCharacter { get; private set; }
    [field: SerializeField] public GameObject RightArmAttackHitBox { get; private set; }    

    [field: Header("VARIOUS DATA AND TESTING REFS")]
    [field: SerializeField] public bool IsTouchingFloor { get; private set; }    
    [field: SerializeField] public float CharacterVelocityMagnitude { get; private set; }
    [field: SerializeField] public float DistanceBetweenCharacterAndFloor { get; private set; }
    [field: SerializeField] public float FloorAngleUnderCharacter { get; set; }
    [field: SerializeField] public GameObject TestingBullet { get; private set; }
    [SerializeField] private ElevatorController m_elevatorController; //TODO brisé
    public Vector3 CharacterVelocity { get; private set; }
    [field: Header("GROUND MOVEMENT VALUES")]
    [field: SerializeField] public float GroundAcceleration { get; private set; }
    [field: SerializeField] public float MaxForwardVelocity { get; private set; }
    [field: SerializeField] public float MaxForwardDiagonalsVelocity { get; set; }
    [field: SerializeField] public float MaxBackwardVelocity { get; private set; }
    [field: SerializeField] public float MaxStrafeVelocity { get; private set; }
    [field: SerializeField] public float DecelerationValue { get; private set; }
    [field: SerializeField] public float TurnSmoothTime { get; private set; }
    
    [field: Header("JUMPING")]
    [field: SerializeField] public float JumpIntensity { get; private set; }
    public Vector3 JumpStartingPosition { get; set; } = Vector3.zero;

    //------------- LEAVING GROUND 
    public Vector3 LeavingGroundStartingPosition { get; set; } = Vector3.zero;

    [field: Header("FALLING PARAMETERS")]
    [field: SerializeField] public float MaxJumpFallingDistance { get; private set; } = 0.0f;
    [field: SerializeField] public float MaxLeavingGroundFallingDistance { get; private set; } = 0.0f;
    [field: SerializeField] public float FallGravity { get; private set; }
    [field: SerializeField] public float FallingAccelerationXZ { get; private set; }

    //------------- ATTACKING
    public bool Attacking { get; set; }   

    
    


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new FallingState());
        m_possibleStates.Add(new LeavingGroundState());
        m_possibleStates.Add(new AttackingState());
        m_possibleStates.Add(new OnGroundState());
        m_possibleStates.Add(new GettingUpState());
        m_possibleStates.Add(new StunInAirState());
        m_possibleStates.Add(new HitState());
                
    }

    protected override void Start()
    {      
        // Quand même checker les statemachine Start car j'ai enlever les base.Start() qu'il y avait dedans mais qui était superflu
        foreach(CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

        Camera = Camera.main;
        Transform = GetComponent<Transform>();

        IsHit = false;
    }

    protected override void Update()
    {
        base.Update();

        IsTouchingFloor = IsInContactWithFloor();
        if (IsInContactWithFloor())
        {            
            Animator.SetBool("TouchGround", true);
        }        
        
        DetectTestingInputs();
        CalculateDistanceBetweenCharacterAndFloor();        

        if (GameManagerSM._Instance.IsCinematicMode == true)
        {
            return;
        }

        //m_currentState.OnUpdate();
        //TryStateTransition();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(!(m_currentState is FreeState))
        {
            RB.AddForce(Vector3.down * FallGravity, ForceMode.Acceleration);
        }        
        
        if (GameManagerSM._Instance.IsCinematicMode == true)
        {
            return;
        }

        CharacterVelocity = RB.velocity;
        CharacterVelocityMagnitude = RB.velocity.magnitude;
        //m_currentState.OnFixedUpdate();        
    }
    
    public bool IsInContactWithFloor()
    {
        return m_floorTrigger.IsOnFloor;
    }

    public void UpdateFreeStateAnimatorValues(Vector2 movementVecValue)
    {
        movementVecValue.Normalize();

        movementVecValue = new Vector2(movementVecValue.x, movementVecValue.y);

        Animator.SetFloat("MoveX", movementVecValue.x);
        Animator.SetFloat("MoveY", movementVecValue.y);      
    }

    private void CalculateDistanceBetweenCharacterAndFloor()
    {
        RaycastHit hit;        

        if (Physics.Raycast(m_floorTrigger.transform.position, -Vector3.up, out hit))
        {
            Debug.DrawRay(m_floorTrigger.transform.position, -Vector3.up * hit.distance, Color.yellow);
            float distanceToGround = hit.distance;
            DistanceBetweenCharacterAndFloor = distanceToGround; 
        }
    }

    

    private void DetectTestingInputs()
    {
        // TODO à ajuster les détails
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 spawnPosition = new Vector3(143, 1, 170);
            GameObject sphere = Instantiate(TestingBullet, spawnPosition, Quaternion.identity);
            sphere.GetComponent<TestBullet>().m_player = this.MainCharacter;           
        }
        if (Input.GetKey(KeyCode.X))
        {
            if ( m_elevatorController != null)
            {                
                m_elevatorController.StartMovingUp();
            }
        }
        if (Input.GetKey(KeyCode.Z))
        {
            if (m_elevatorController != null)
            {                
                m_elevatorController.StartMovingDown();
            }
        }        
    }
}



//[field: Header("Bonjour")]
//[field: SerializeField, Header("Hi")]



//private void EvaluateIfLosingAltitude()
//{
//    float elevationDiff = DistanceBetweenCharacterAndFloor - m_previousElevation;
//
//    m_previousElevation = DistanceBetweenCharacterAndFloor;
//
//    if (elevationDiff >= 0)
//    {
//        IsLosingAltitude = false;
//    }
//    if (elevationDiff < 0)
//    {
//        IsLosingAltitude = true;
//    }
//}