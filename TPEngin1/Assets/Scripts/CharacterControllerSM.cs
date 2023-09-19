using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerSM : MonoBehaviour
{
    
    private CharacterState m_currentState;
    private List<CharacterState> m_possibleStates;

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;
        
    // Les variables deviennent des cSharp fields
    // fields en cSharp sont des mt�hodes qui nous permettent d'aller chercher de l'info d'ou la majuscule
    public Camera Camera { get; private set; }
    public Transform Transform { get; private set; }

    [field: SerializeField]
    public Rigidbody RB { get; private set; }

    [field: SerializeField]
    public GameObject GameObject { get; private set; }

    [field: SerializeField]
    private Animator Animator { get; set; }

    [field: SerializeField]
    public float DistanceBetweenCharacterAndFloor { get; private set; }
    [field: SerializeField]
    public float GroundAccelerationValue { get; private set; }
    [field: SerializeField]
    public float JumpAccelerationValue { get; private set; }
    [field: SerializeField]
    public float MaxJumpAccelerationValue { get; private set; }
    //[field: SerializeField]    
    //public float ForwardAccelerationValue { get; private set; }    
    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }
    
    //[field: SerializeField]
    //public float ForwardDiagonalsAccelerationValue { get; private set; }    
    [field: SerializeField]
    public float MaxForwardDiagonalsVelocity { get; set; }
    
    //[field: SerializeField]
    //public float BackwardAccelerationValue { get; private set; }    
    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; } 
    
    //[field: SerializeField]
    //public float StrafeAccelerationValue { get; private set; }    
    [field: SerializeField]
    public float MaxStrafeVelocity { get; private set; }
    
    [field: SerializeField]
    public float DecelerationValue { get; private set; }

    [field: SerializeField]
    public float TurnSmoothTime { get; private set; } // Lower number means snappier turn

    [field: SerializeField]
    public float JumpIntensity { get; private set; }



    private void Awake()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
    }

    void Start()
    {
        // Sans le serializeField on peut garder la r�f�rence priv�e, et on va chercher directement la cam�ra
        Camera = Camera.main;       
        Transform = GetComponent<Transform>();

        foreach(CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();        
    }
    
    private void Update()
    {
        CalculateDistanceBetweenCharacterAndFloor();
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    private void FixedUpdate()
    {        
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
            if (state.CanEnter())
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

    public void UpdateAnimatorValues(Vector2 movementVecValue)
    {
        // Aller chercher ma vitesse actuelle
        // Communiquer directement avec mon animator

        //movementVecValue.Normalize();

        // movementVecValue = new Vector2(movementVecValue.x, movementVecValue.y / MaxVelocity)

        Animator.SetFloat("MoveX", movementVecValue.x);
        Animator.SetFloat("MoveY", movementVecValue.y);
        





    }

    private void CalculateDistanceBetweenCharacterAndFloor()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            float distanceToGround = hit.distance;
            DistanceBetweenCharacterAndFloor = distanceToGround;                        
            //Debug.Log("Distance to ground: " + distanceToGround);
        }
    }


}



