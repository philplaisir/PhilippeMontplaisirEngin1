using UnityEngine;

public class JumpState : CharacterState
{
    private Animator m_animator;
    private const float STATE_EXIT_TIMER = 0.2f;
    private float m_currentStateTimer = 0.0f;    

    public override void OnEnter()
    {
        Debug.Log("Character entering state: JumpState\n");

        m_stateMachine.JumpStartingPosition = m_stateMachine.transform.position;
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        m_currentStateTimer = STATE_EXIT_TIMER;
        m_stateMachine.UpdateFreeStateAnimatorValues(new Vector2(0, 0));
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetTrigger("Jump");         
    }

    public override void OnExit()
    {        
        Debug.Log("Character exiting state: JumpState\n");
    }    

    public override void OnUpdate()
    {
        m_animator.SetBool("TouchGround", false);
        m_currentStateTimer -= Time.deltaTime;        
    }

    public override void OnFixedUpdate()
    {        
        CharacterControllerJumpFU();
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }

    //888888888888888888888888888888888888888888888888888888888

    private void CharacterControllerJumpFU()
    {        
        Vector3 movementVector = Vector3.zero;
        Vector3 projectedVectorForward = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        Vector3 projectedVectorRight = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);

        if (Input.GetKey(KeyCode.W))
        {
            movementVector += projectedVectorForward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector += -projectedVectorForward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector += -projectedVectorRight;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector += projectedVectorRight;
        }

        movementVector.Normalize();

        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.FallingAccelerationXZ, ForceMode.Acceleration);                
    }
    
}
