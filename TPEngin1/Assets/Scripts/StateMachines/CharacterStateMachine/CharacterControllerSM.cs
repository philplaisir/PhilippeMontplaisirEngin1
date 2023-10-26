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
    
    public bool Attacking { get; set; } = false;
    [Header("COMBAT PARAMETERS")]
    [SerializeField] private List<PMM_HitBox> m_hittingHitBoxes = new List<PMM_HitBox>();
    [SerializeField] private List<PMM_HitBox> m_receivingHitBoxes = new List<PMM_HitBox>();
    [SerializeField] private CharacterSpecialFXManager m_characterSpecialFXManager;

    [field: Header("TESTING PARAMETERS")]
    [SerializeField] private GameObject m_explosionParticleSystem;
    [SerializeField] private AudioSource m_explosionAudioSource;
    [SerializeField] private GameObject m_explosionEmitterPos;
    [field: SerializeField] public GameObject TestingBullet { get; private set; }
    [SerializeField] private ElevatorController m_elevatorController;




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

    protected override void Awake()
    {
        base.Awake();

        // Bien checker si c'est une bonne pratique utiliser le invoke et UnityEvent
        InitializeHittingHitBoxListeners();
        InitializeReceivingHitBoxListeners();
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

    private void InitializeHittingHitBoxListeners()
    {
        for (int i = 0; i < m_hittingHitBoxes.Count; i++)
        {
            if (m_hittingHitBoxes[i] != null)
            {
                m_hittingHitBoxes[i].IsHitting.AddListener(IsHitting);
            }
        }
    }

    private void IsHitting(Vector3 position, PMM_HitBox self, PMM_HitBox other)
    {
        //TODO check si besoin des hit et serait cool de transférer le action type dès la hitbox ou dépendamment de la hit box reçue
        m_characterSpecialFXManager.PlaySpecialEffect(ECharacterActionType.PunchRight, position);
    }

    private void InitializeReceivingHitBoxListeners()
    {
        for (int i = 0; i < m_receivingHitBoxes.Count; i++)
        {
            if (m_receivingHitBoxes[i] != null)
            {
                m_receivingHitBoxes[i].WasHit.AddListener(WasHit);
            }
        }
    }

    private void WasHit()
    {
        IsHit = true;
    }

    private void DetectTestingInputs()
    {
        // TODO à ajuster les détails
        if (Input.GetKeyDown(KeyCode.V))
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioSource newAudioSource = Instantiate(m_explosionAudioSource, m_explosionEmitterPos.transform.position, Quaternion.identity, transform);
            AudioClip clipToPlay = newAudioSource.clip;
            newAudioSource.PlayOneShot(clipToPlay);
            Instantiate(m_explosionParticleSystem, m_explosionEmitterPos.transform.position, Quaternion.identity, transform);            
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