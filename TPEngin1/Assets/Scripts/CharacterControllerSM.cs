using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerSM : MonoBehaviour
{
    private CharacterState m_currentState;
    private List<CharacterState> m_possibleStates;

    [SerializeField] private CharacterFloorTrigger m_floorTrigger;
    [SerializeField] private ElevatorController m_elevatorController;

    public Camera Camera { get; private set; }
    public Transform Transform { get; private set; }

    public bool IsHit { get; set; }
    public bool IsJumpingForTooLong { get; set; }
    public bool IsLosingAltitude { get; private set; }
    private float m_previousElevation = 0.0f;         
    
    [field: SerializeField] public Rigidbody RB { get; private set; }
    [field: SerializeField] public GameObject GameObject { get; private set; }
    [field: SerializeField] private Animator Animator { get; set; }

    [field: SerializeField] public bool IsTouchingFloor { get; private set; }    
    [field: SerializeField] public float CharacterVelocity { get; private set; }
    [field: SerializeField] public float DistanceBetweenCharacterAndFloor { get; private set; }
    [field: SerializeField] public float FloorAngleUnderCharacter { get; set; }
    [field: SerializeField] public float GroundAcceleration { get; private set; }
    [field: SerializeField] public float FallingAccelerationXZ { get; private set; }       
    [field: SerializeField] public float MaxForwardVelocity { get; private set; }
    [field: SerializeField] public float MaxForwardDiagonalsVelocity { get; set; }
    [field: SerializeField] public float MaxBackwardVelocity { get; private set; }
    [field: SerializeField] public float MaxStrafeVelocity { get; private set; }
    [field: SerializeField] public float DecelerationValue { get; private set; }
    [field: SerializeField] public float TurnSmoothTime { get; private set; }

    // JUMPING
    [field: SerializeField] public float JumpIntensity { get; private set; }
    [field: SerializeField] public float FallGravity { get; private set; }

    // ATTACKING
    public bool Attacking { get; set; }   

    // TESTING
    [field: SerializeField] public GameObject TestingBullet { get; private set; }
    public bool IsStunned { get; set; }
    


    private void Awake()
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

    void Start()
    {        
        Camera = Camera.main;       
        Transform = GetComponent<Transform>();

        foreach(CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

        m_previousElevation = DistanceBetweenCharacterAndFloor;

        IsStunned = false;
        IsHit = false;
    }
    
    private void Update()
    {
        IsTouchingFloor = IsInContactWithFloor();
        if (IsInContactWithFloor())
        {            
            Animator.SetBool("TouchGround", true);
        }        
        
        DetectTestingInputs();
        CalculateDistanceBetweenCharacterAndFloor();
        EvaluateIfLosingAltitude();
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    private void FixedUpdate()
    {        
        CharacterVelocity = RB.velocity.magnitude;
        m_currentState.OnFixedUpdate();        
    }

    private void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        //Je PEUX quitter le state actuel
        foreach (var state in m_possibleStates)
        {
            if (m_currentState == state)
            {
                continue;
            }
            if (state.CanEnter(m_currentState))
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
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

    private void EvaluateIfLosingAltitude()
    {
        float elevationDiff = DistanceBetweenCharacterAndFloor - m_previousElevation;

        m_previousElevation = DistanceBetweenCharacterAndFloor;

        if (elevationDiff >= 0) 
        {
            IsLosingAltitude = false;
        }
        if (elevationDiff < 0)
        {
            IsLosingAltitude = true;
        }
    }

    private void DetectTestingInputs()
    {
        // TODO à ajuster les détails
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 spawnPosition = new Vector3(143, 1, 170);
            GameObject sphere = Instantiate(TestingBullet, spawnPosition, Quaternion.identity);
            sphere.GetComponent<TestBullet>().m_player = this.GameObject;           
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsStunned = true;
        }
    }
}



