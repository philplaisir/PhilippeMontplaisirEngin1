using UnityEngine;

public class JumpState : CharacterState
{
    private Animator m_animator;
    private const float STATE_EXIT_TIMER = 0.2f;
    private float m_currentStateTimer = 0.0f;       
    private float m_losingAltitudeTimer = 0.0f;    
     
        

    public override void OnEnter()
    {
        Debug.Log("Enter state: JumpState\n");
                
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        m_currentStateTimer = STATE_EXIT_TIMER;
        m_stateMachine.UpdateFreeStateAnimatorValues(new Vector2(0, 0));
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetTrigger("Jump");        
        m_losingAltitudeTimer = 0.115f;        
        m_stateMachine.InJumpStateForTooLong = false;
    }

    public override void OnExit()
    {
        m_stateMachine.InJumpStateForTooLong = false;
        Debug.Log("Exit state: JumpState\n");
    }    

    public override void OnUpdate()
    {
        m_animator.SetBool("TouchGround", false);
        m_currentStateTimer -= Time.deltaTime;
        
        if (m_stateMachine.IsLosingAltitude)
        {
            m_losingAltitudeTimer -= Time.deltaTime;

        }       

        if (m_losingAltitudeTimer < 0)
        {
            m_stateMachine.InJumpStateForTooLong = true;
        }   
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.RB.AddForce(Vector3.down * m_stateMachine.FallGravity, ForceMode.Acceleration);
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
